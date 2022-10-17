using System.Text;

namespace ShiverBot.Imaging
{
    internal class CanolaPost
    {
        private readonly Bitmap _bitmap;
        private readonly string[] _clickSequence;
        private int clickSeqPointer;

        internal bool EndOfSequence => clickSeqPointer == 119;

        internal CanolaPost(Bitmap bitmap)
        {
            _bitmap = bitmap;
            _clickSequence = ToClickSequence();
            clickSeqPointer = 0;
        }

        internal string? GetNextClickSequence()
        {
            if (EndOfSequence)
            {
                return null;
            }

            return _clickSequence[clickSeqPointer++];
        }

        internal void ResetPointer()
        {
            clickSeqPointer = 0;
        }

        private string[] ToClickSequence()
        {
            string[] output = new string[120];
            StringBuilder sb = new();

            for (int y = 0; y < _bitmap.Height; y++)
            {
                if (y % 2 == 0)
                {
                    for (int x = 0; x < _bitmap.Width; x++)
                    {
                        if (_bitmap.GetPixel(x, y) == Color.FromArgb(255, 0, 0, 0))
                        {
                            sb.Append("A,W50,");
                        }
                        sb.Append("DR,W50,");

                    }
                }
                else
                {
                    for (int x = _bitmap.Width - 1; x >= 0; x--)
                    {
                        if (_bitmap.GetPixel(x, y) == Color.FromArgb(255, 0, 0, 0))
                        {
                            sb.Append("A,W50,");
                        }
                        sb.Append("DL,W50,");
                    }
                }
                sb.Append("DD");
                output[y] = sb.ToString();
                sb.Clear();
            }

            return output;
        }
    }
}