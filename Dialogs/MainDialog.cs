using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using MyEchoBot.Dialogs;

namespace MyEchoBot{
    public class MainDialog : ComponentDialog
    {
        public MainDialog(string dialogId) : base(dialogId)
        {
            var waterFallSteps = new WaterfallStep[]
            {
                NameStepAsync,
                ExpYearsStepAsync,

            };
            
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationtoken)
        {
            return await stepContext.BeginDialogAsync(nameof(TopLevelDialog), null, cancellationtoken);
        }
        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationtoken)
        {
            var finishString = "Final Step executed, steps finished.";
            await stepContext.Context.SendActivityAsync(finishString);
            return await stepContext.EndDialogAsync(null, cancellationtoken);
        }

    }

}


