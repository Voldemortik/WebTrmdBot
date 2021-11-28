using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TegBotTrmd.Entity;
using TegBotTrmd.IRepository;
using TegBotTrmd.Repository;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WebTrmdBot.AbsCommands;

namespace WebTrmdBot.Controllers
{
    [ApiController]
    [Route("api/message/update")]
    public class BotController : Controller
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly ICommandService _commandService;


        private readonly IUserRepository userdb;
        private readonly IProductRepository productdb;
        private readonly IOrderRepository orderdb;
        public BotController(ICommandService commandService, ITelegramBotClient telegramBotClient, IProductRepository prodrepo, IOrderRepository orderRepo, IUserRepository userRepo)
        {
            _commandService = commandService;
            _telegramBotClient = telegramBotClient;
            productdb = prodrepo;
            orderdb = orderRepo;
            userdb = userRepo;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Started");
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            if (update == null) return Ok();

            try
            {
                if (update.CallbackQuery != null)
                {
                    var callBackObj = update.CallbackQuery;
                    var callBackObjInlineMsg = update.CallbackQuery.Message.Text;
                    var callBackObjInlineMsgId = update.CallbackQuery.Data;

                    Console.WriteLine(update.CallbackQuery.Message.Text);
                    Console.WriteLine(update.CallbackQuery.Data);


                    switch (callBackObjInlineMsgId)
                    {
                        case "addToCart":
                            var product = callBackObjInlineMsg.Split(":");
                            var productName = product.First();
                            var productPrice = product.Last();
                            var prodDb = productdb.GetProductByNameAndPrice(productName, decimal.Parse(productPrice));
                            var chatId = update.CallbackQuery.Message.Chat.Id;
                            var userDb = userdb.GetUserByChatId(chatId);
                            var unpaidUserOrder = orderdb.GetOrderListByUser(userDb.Id).Where(x => !x.IsPaid).FirstOrDefault();

                            if (unpaidUserOrder == null)
                            {
                                var newOrder = new Order()
                                {
                                    Products = new List<Product>() { prodDb },
                                    Customer = userDb,
                                    IsPaid = false
                                };
                                orderdb.Create(newOrder);
                            }
                            else
                            {
                                unpaidUserOrder.Products.Add(prodDb);
                            }
                            orderdb.Save();
                            await _telegramBotClient.SendTextMessageAsync(chatId, "Добавлено в корзину",
                                parseMode: ParseMode.Html, null, false, false, 0, null, null);
                            break;
                        case "DeleteFromOrder":
                            var splited = callBackObjInlineMsg.Split(":");
                            var orderId = splited.First().Split("_").First();
                            productName = splited.First().Split("_").Last();
                            productPrice = splited.Last();
                            unpaidUserOrder = orderdb.GetOrder(Int32.Parse(orderId));

                            if (unpaidUserOrder != null)
                            {
                                var tmpProd = unpaidUserOrder.Products.Where(x => x.Name == productName && x.Price == decimal.Parse(productPrice)).FirstOrDefault();
                                if (tmpProd != null)
                                {
                                    unpaidUserOrder.Products.Remove(tmpProd);
                                }
                                else
                                {
                                    await _telegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Не нашeлся продукт",
                                parseMode: ParseMode.Html, null, false, false, 0, null, null);
                                    return Ok();
                                }
                            }
                            else
                            {
                                await _telegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Не нашeлся ордер",
                                parseMode: ParseMode.Html, null, false, false, 0, null, null);
                                return Ok();
                            }
                            orderdb.Save();
                            await _telegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Продукт удален из корзины",
                                parseMode: ParseMode.Html, null, false, false, 0, null, null);
                            break;
                        case "PayOrder":
                            splited = callBackObjInlineMsg.Split(":");
                            orderId = splited.Last();
                            unpaidUserOrder = orderdb.GetOrder(Int32.Parse(orderId));
                            chatId = update.CallbackQuery.Message.Chat.Id;
                            userDb = userdb.GetUserByChatId(chatId);
                            var userBalance = userDb.Balance;

                            if (unpaidUserOrder != null)
                            {
                                var sum = unpaidUserOrder.Products.Select(x => x.Price).Sum();
                                if (userBalance < sum)
                                {
                                    await _telegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Недостаточно денег на счету. Пополните баланс!",
                               parseMode: ParseMode.Html, null, false, false, 0, null, null);
                                    return Ok();
                                }
                                else
                                {
                                    userDb.Balance -= sum;
                                    unpaidUserOrder.IsPaid = true;
                                    foreach (var item in unpaidUserOrder.Products.ToList())
                                    {
                                        productdb.Delete(item.Id);
                                    }
                                }
                            }
                            else
                            {
                                await _telegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Не нашeлся ордер",
                                parseMode: ParseMode.Html, null, false, false, 0, null, null);
                            }
                            userdb.Save();
                            orderdb.Save();
                            await _telegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Ордер оплачен",
                                parseMode: ParseMode.Html, null, false, false, 0, null, null);
                            break;
                        case "AddBalance":
                            chatId = update.CallbackQuery.Message.Chat.Id;
                            userDb = userdb.GetUserByChatId(chatId);
                            userDb.Balance += 300;

                            userdb.Save();
                            await _telegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Баланс пополнен",
                                parseMode: ParseMode.Html, null, false, false, 0, null, null);
                            break;
                        default:
                            break;
                    }
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                await _telegramBotClient.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, "Внутренняя ошибка((",
                                parseMode: ParseMode.Html, null, false, false, 0, null, null);
            }

            
            var message = update.Message;


            Console.WriteLine(message.Text);
            foreach (var command in _commandService.Get())
            {
                if (command.Contains(message))
                {
                    await command.Execute(message, _telegramBotClient, orderdb, userdb, productdb);
                    break;
                }
            }
            return Ok();
        }
    }
}
