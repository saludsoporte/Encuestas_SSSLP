using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EncuestasApp.Behaviors
{
    public class BloquearEmojisBehavior : Behavior<Entry>
    {
        private static readonly Regex EmojiRegex = new Regex(
            @"\p{So}|\p{Cs}|\p{Cn}|[\u2600-\u27BF]|[\uD83C-\uDBFF]|[\uDC00-\uDFFF]|❤",
            RegexOptions.Compiled);

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;

            if (!string.IsNullOrEmpty(e.NewTextValue) && ContieneEmojis(e.NewTextValue))
            {
                // Revertir al texto anterior sin emojis
                entry.Text = RemoverEmojis(e.NewTextValue);
            }
        }

        private bool ContieneEmojis(string texto)
        {
            return EmojiRegex.IsMatch(texto);
        }

        private string RemoverEmojis(string texto)
        {
            return EmojiRegex.Replace(texto, "");
        }
    }
}
