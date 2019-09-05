using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using MyEchoBot.Dialogs;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using MyEchoBot.Entities;

namespace MyEchoBot{
    public class MainDialog : ComponentDialog
    {
        
        private readonly UserState _userstate;
        public MainDialog(UserState userState, BotMLContext botmlContext) : base(nameof(MainDialog))
        {
            _userstate = userState;
            AddDialog(new UserProfileDialog(botmlContext));

            AddDialog(new WaterfallDialog(nameof(WaterfallStep), new WaterfallStep[]
            {
                InitialStepAsync,
                FinalStepAsync,
            }));
            InitialDialogId = nameof(WaterfallStep);
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationtoken)
        {
            return await stepContext.BeginDialogAsync(nameof(UserProfileDialog), null, cancellationtoken);
        }
        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationtoken)
        {
            var user = stepContext.Values["UserInfo"] as UserProfile;
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"sir  this conversation is ending"));
            return await stepContext.EndDialogAsync(null, cancellationtoken);
        }

    }

}


