using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLID_D2_solve
{
    // interfaces
    public interface IPlayAudio
    {
        void PlayAudio();
    }
    public interface IPlayVideo
    {
        void PlayVideo();
    }
    public interface IDisplaySubtitles
    {
        void DisplaySubtitles();
    }
    public interface ILoadMedia
    {
        void LoadMedia(string filePath);
    }

    // classes
    public class AudioPlayer : IPlayAudio, ILoadMedia
    {

        public void PlayAudio()
        {
            // Code to play audio
        }
        public void LoadMedia(string filePath)
        {
            // Code to load audio file
        }

    }

    public class VideoPlayer : IPlayVideo, ILoadMedia
    {

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
