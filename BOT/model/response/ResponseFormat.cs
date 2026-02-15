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

        public static string FormatUserInfo(UserInfo userInfo)
        {
            if (userInfo == null)
                return "Error fetching user info.";
            var sb = new StringBuilder();
            sb.AppendLine($"*Username:* {userInfo.Username}");
            sb.AppendLine($"*Money:* {userInfo.Money} 💰");
            sb.AppendLine($"*Last Worked:* {userInfo.LastWorked}");
            return sb.ToString();
        }

        public static string FormatCharacters(List<Character>? characters)
        {
            if (characters == null || characters.Count == 0)
                return "❌ You have no characters yet.";

            var sb = new StringBuilder();
            sb.AppendLine("🎯 *Your Characters* 🎯\n");

            foreach (var character in characters)
            {
                sb.AppendLine($"*ID:* {character.Id}");
                sb.AppendLine($"*Name:* {character.Name}");
                sb.AppendLine($"*Status:* {character.Status}");
                sb.AppendLine($"*Species:* {character.Species}");
                sb.AppendLine($"*Gender:* {character.Gender}");
                sb.AppendLine($"*For Sale:* {(character.ForSale ? "Yes ✅" : "No ❌")}");
                if (character.Price > 0)
                    sb.AppendLine($"*Price:* {character.Price} 💰");

                sb.AppendLine("------------------------------");
            }

            return sb.ToString();
        }

        public static string FormatEpisodes(List<Episode>? episodes)
        {
            if (episodes == null || episodes.Count == 0)
                return "❌ You have no episodes yet.";

            var sb = new StringBuilder();
            sb.AppendLine("🎯 *Your Episodes* 🎯\n");

            foreach (var episode in episodes)
            {
                sb.AppendLine($"*ID:* {episode.Id}");
                sb.AppendLine($"*Name:* {episode.Name}");
                sb.AppendLine($"*Air Date:* {episode.AirDate}");
                sb.AppendLine($"*Episode:* {episode.EpisodeCode}");
                sb.AppendLine($"*Created:* {episode.Created}");
                sb.AppendLine($"*For Sale:* {(episode.ForSale ? "Yes ✅" : "No ❌")}");
                if (episode.Price > 0)
                    sb.AppendLine($"*Price:* {episode.Price} 💰");

                sb.AppendLine("------------------------------");
            }

            return sb.ToString();
        }
    }
}