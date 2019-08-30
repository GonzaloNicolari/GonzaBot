using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using MyEchoBot;
using Microsoft.Bot.Builder;

namespace MyEchoBot.Dialogs
{
    public class UserProfileDialog : ComponentDialog
    {
        private readonly IStatePropertyAccessor<UserProfile> _userProfileAccessor;
        public UserProfileDialog() : base(nameof(UserProfileDialog))
        {
            
            AddDialog(new TextPrompt("NamePrompt"));
            AddDialog(new NumberPrompt<int>("ExpYearsPrompt"));
            AddDialog(new TextPrompt("CatPrompt"));
            AddDialog(new NumberPrompt<int>("CatExpYearsPrompt"));
            AddDialog(new TextPrompt("LastEvalLtrPrompt"));
            AddDialog(new ConfirmPrompt("ConfirmPrompt"));
            AddDialog(new BotReviewDialog());
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                NameStepAsync,
                ExpYearsStepAsync,
                CatStepAsync,
                CatExpYearsStepAsync,
                LastEvalLtrStepAsync,
                SummaryStepAsync
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private static async Task<DialogTurnResult> NameStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationtoken)
        {
            //Create an object to collect user info
            stepContext.Values["UserInfo"] = new UserProfile();
            
            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Please enter your name") };

            //Ask the user to enter their name.
            return await stepContext.PromptAsync("NamePrompt", promptOptions, cancellationtoken);
           
        }
        private static async Task<DialogTurnResult> ExpYearsStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationtoken) {
            var user = stepContext.Values["UserInfo"] as UserProfile;

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Please enter your experience years") };
            //collect user name.
            //user.Name = (string)stepContext.Result;
            //Ask the user to enter their exp years.
            return await stepContext.PromptAsync("ExpYearsPrompt", promptOptions, cancellationtoken);
        }

        private static async Task<DialogTurnResult> CatStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationtoken)
        {
            var user = stepContext.Values["UserInfo"] as UserProfile;

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Please enter your category") };
            //collect user exp years.
            user.ExpYears = (int)stepContext.Result;
            //Ask the user to enter their category.
            return await stepContext.PromptAsync("CatPrompt", promptOptions, cancellationtoken);
        }
        private static async Task<DialogTurnResult> CatExpYearsStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationtoken)
        {
            var user = stepContext.Values["UserInfo"] as UserProfile;

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Please enter your experience years in your category") };
            //collect user category.
            user.Category = (float)stepContext.Result;
            //Ask the user to enter their year exp in his category.
            return await stepContext.PromptAsync("CatExpYearsPrompt", promptOptions, cancellationtoken);
        }
        private static async Task<DialogTurnResult> LastEvalLtrStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationtoken)
        {
            var user = stepContext.Values["UserInfo"] as UserProfile;

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Please enter your last evaluation letter") };
            //collect user exp years in his category.
            user.CategoryExpYears = (int)stepContext.Result;
            if (user.CategoryExpYears > 5)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("please complete the next survey about bot service"));
                await stepContext.BeginDialogAsync(nameof(BotReviewDialog), null, cancellationtoken);
            }
            //Ask the user to enter their last evaluation letter.
            return await stepContext.PromptAsync("LastEvalLtrPrompt", promptOptions, cancellationtoken);
        }
        private static async Task<DialogTurnResult> SummaryStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var user = stepContext.Values["UserInfo"] as UserProfile;
            //collect user last evaluation letter.
            user.LastEvalLetter = (float)stepContext.Result;

            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Your Name is USERNAME{Environment.NewLine}Your exp years are {user.ExpYears}{Environment.NewLine}Your category is {user.Category}{Environment.NewLine}Your experience years in your category are {user.CategoryExpYears}{Environment.NewLine}And your last evaluation letter is {user.LastEvalLetter}"));
            
            
            return await stepContext.EndDialogAsync(user, cancellationToken);
        }
    }
}
