using FineBot.BotRunner.Extensions;
using FineBot.Enums;
using MargieBot.Models;
using NUnit.Framework;

namespace FineBot.Tests.BotRunnerTests.ExtensionsTests
{
    [TestFixture]
    public class SlackMessageExtensionsTests
    {
        [Test]
        [TestCase("Hey check this out <https://www.youtube.com/watch?v=I9G9sTqMBIg>")]
        [TestCase("Super awesome video<https://youtu.be/I9G9sTqMBIg>")]
        [TestCase("<http://www.youtube.com/watch?v=YqeW9_5kURI>")]
        [TestCase("<http://m.youtube.com/watch?v=YqeW9_5kURI>")]
        [TestCase("<http://android-app//com.google.android.youtube/http/www.youtube.com/watch?v=YqeW9_5kURI>")]
        [TestCase("<http://ios-app//544007664/vnd.youtube/www.youtube.com/watch?v=YqeW9_5kURI>")]
        public void GivenASlackMessageWhenThereIsAYouTubeLinkThenReturnTrue(string text)
        {
            var slackMessage = new SlackMessage {Text = text};

            var isYouTubeLink = slackMessage.IsYouTubeLink();
            
            Assert.AreEqual(true, isYouTubeLink);
        }

        [Test]
        [TestCase("Hey check this out not youtube link <http://www.google.com>")]
        [TestCase("Gizmodo article <http://gizmodo.com/youtube-will-soon-support-360-degree-video-uploads-1677627074>")]
        public void GivenASlackMessageWhenThereIsNotAYouTubeLinkThenReturnFalse(string text)
        {
            var slackMessage = new SlackMessage {Text = text};

            var isYouTubeLink = slackMessage.IsYouTubeLink();

            Assert.AreEqual(false, isYouTubeLink);
        }

    }
}
