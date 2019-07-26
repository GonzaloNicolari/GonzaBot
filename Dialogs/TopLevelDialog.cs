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
    public class TopLevelDialog : ActivityHandler
    {
        private static async Task<DialogTurnResult> NameStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationtoken)
        {
            //Create an object to collect user info
            stepContext.Values["UserInfo"] = new UserProfile();

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Please enter your name.") };

            //Ask the user to enter their name.
            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationtoken);
           
        }
        private static async Task<DialogTurnResult> ExpYearsStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationtoken) {
            var user = stepContext.Values["UserInfo"] as UserProfile;

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Please enter your age.") };

            user.Name = (string)stepContext.Result;

            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationtoken);
        }

        private static async Task<DialogTurnResult> ExpYearsStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationtoken)
        {
            var user = stepContext.Values["UserInfo"] as UserProfile;

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("Please enter your Category.") };

            user.Name = (int)stepContext.Result;

            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationtoken);
        }
    }
}
