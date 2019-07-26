using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using MyEchoBot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Logging;

namespace MyEchoBot.Bots
{
    public class GonzaBot : ActivityHandler
    {
        //public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    await ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
        //    await UserState.SaveChangesAsync(turnContext, false, cancellationToken);
        //}
        //protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        //{
        //    Logger.LogInformation("Running dialog with Message Activity.");

        //    await Dialog.RunAsync(turnContext, ConversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken)
        //}
    }
}
