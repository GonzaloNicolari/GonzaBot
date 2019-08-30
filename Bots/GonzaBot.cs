using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using MyEchoBot.Dialogs;
using Microsoft.Bot.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Bot.Schema;
using NUnit.Framework.Internal;
using Microsoft.Bot.Builder.Dialogs;
#pragma warning disable CS0105 // The using directive for 'Microsoft.Extensions.Logging' appeared previously in this namespace
using Microsoft.Extensions.Logging;
#pragma warning restore CS0105 // The using directive for 'Microsoft.Extensions.Logging' appeared previously in this namespace


namespace MyEchoBot.Bots
{
    public class GonzaBot<T> : ActivityHandler where T : Dialog
    {
        private readonly BotState _userState;
        private readonly BotState _conversationState;
        protected readonly Dialog Dialog;
        protected readonly Microsoft.Extensions.Logging.ILogger Logger;
        public GonzaBot(ConversationState conversationState, UserState userState, T dialog, ILogger<GonzaBot<T>> logger)
        {
            _conversationState = conversationState;
            _userState = userState;
            Dialog = dialog;
            Logger = logger;
        }


        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            
            await base.OnTurnAsync(turnContext, cancellationToken);
            
            await _conversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await _userState.SaveChangesAsync(turnContext, false, cancellationToken);
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            
            Logger.LogInformation("Running dialog with Message Activity.");

            await Dialog.RunAsync(turnContext, _conversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken);
        }
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Hello and welcome to GonzaBot! One of the most usefull bots you'll see in your life!"), cancellationToken);
                }
            }
        }

    }
}
