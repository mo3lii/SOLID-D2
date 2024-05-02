using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLID_D2
{
    public interface IMediaPlayer
    {

        void PlayAudio();
        void PlayVideo();
        void DisplaySubtitles();
        void LoadMedia(string filePath);

    }

    public class AudioPlayer : IMediaPlayer
    {

        public void PlayAudio()
        {
            // Code to play audio
        }

        public void PlayVideo()
        {

            throw new NotImplementedException("Audio players cannot play videos");


        }

        public void DisplaySubtitles()
        {

            throw new NotImplementedException("Audio players cannot display subtitles");


        }

        public void LoadMedia(string filePath)
        {
            // Code to load audio file
        }

    }

    public class VideoPlayer : IMediaPlayer
    {

        public void PlayAudio()
        {

            throw new NotImplementedException("Video players cannot play audio");

        }

        public void PlayVideo()
        {
            // Code to play video
        }

        public void DisplaySubtitles()
        {
            // Code to display subtitles
        }

        public void LoadMedia(string filePath)
        {
            // Code to load media file
        }

    }

}
