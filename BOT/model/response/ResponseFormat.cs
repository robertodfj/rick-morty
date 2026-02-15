using System.Collections.Generic;
using System.Text;
using Bot.model;

namespace Bot.model.response
{
    public static class Formatter
    {
        public static string FormatMarketItems(List<MarketItem>? items)
        {
            if (items == null || items.Count == 0)
                return "No items found in the market.";

            var sb = new StringBuilder();
            sb.AppendLine("🛒 *Market Items* 🛒\n");

            foreach (var item in items)
            {
                sb.AppendLine($"*Name:* {item.Name}");
                sb.AppendLine($"*Type:* {item.Type}");
                sb.AppendLine($"*Price:* {item.Price} 💰");
                sb.AppendLine($"*For Sale:* {(item.ForSale ? "Yes ✅" : "No ❌")}");

                if (!string.IsNullOrEmpty(item.ExtraInfo))
                    sb.AppendLine($"*Info:* {item.ExtraInfo}");

                sb.AppendLine("------------------------------");
            }

            return sb.ToString();
        }

        public static string FormaCaptureCharacter(CaptureCharacter character)
        {
            if (character == null)
                return "Error captturing the character, work to have more options to capture characters in the future.";
            var sb = new StringBuilder();
            sb.AppendLine($"*Name:* {character.Name}");
            sb.AppendLine($"*Status:* {character.Status}");
            sb.AppendLine($"*Species:* {character.Species}");
            sb.AppendLine($"*Gender:* {character.Gender}");
            sb.AppendLine($"*For Sale:* {(character.ForSale ? "Yes ✅" : "No ❌")}");
            sb.AppendLine($"*Price:* {character.Price}💰");
            return sb.ToString();
        }

        public static string FormaCaptureEpisode(CaptureEpisode episode)
        {
            if (episode == null)
                return "Error capturing the episode, work to have more options to capture episodes in the future.";
            var sb = new StringBuilder();
            sb.AppendLine($"*Name:* {episode.Name}");
            sb.AppendLine($"*Air Date:* {episode.AirDate}");
            sb.AppendLine($"*Episode:* {episode.Episode}");
            sb.AppendLine($"*Created:* {episode.Created}");
            sb.AppendLine($"*For Sale:* {(episode.ForSale ? "Yes ✅" : "No ❌")}");
            sb.AppendLine($"*Price:* {episode.Price}💰");
            return sb.ToString();
        }
    }}