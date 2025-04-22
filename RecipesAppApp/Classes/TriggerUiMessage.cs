using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace RecipesAppApp.Classes
{
    public class TriggerUiMessage : ValueChangedMessage<string>
    {
        public TriggerUiMessage(string value) : base(value) { }
    }
}
