using System.Text;

namespace ShiverBot.Imaging
{
    internal class CanolaPost
    {
        private readonly Bitmap _bitmap;
        private readonly string[] _clickSequence;
        private int clickSeqPointer;
        private int maxPointer;

        internal bool EndOfSequence => clickSeqPointer == maxPointer;

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

        private bool LineIsEmpty(int line, bool vertical)
        {
            if (vertical)
            {
                for (int y = 0; y < _bitmap.Height; y++)
                {
                    if (_bitmap.GetPixel(line, y) == Color.FromArgb(255, 0, 0, 0))
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int x = 0; x < _bitmap.Width; x++)
                {
                    if (_bitmap.GetPixel(x, line) == Color.FromArgb(255, 0, 0, 0))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private string[] ToClickSequence()
        {
            StringBuilder sb = new();

            string[] verticalOutput = new string[320];
            string[] horizontalOutput = new string[120];
            int verticalLength = 0;
            int horizontalLength = 0;
            bool currentDirection = true;

            // Get vertical path
            for (int x = 0; x < _bitmap.Width; x++)
            {
                if (!LineIsEmpty(x, true))
                {
                    if (currentDirection)
                    {
                        for (int y = 0; y < _bitmap.Height; y++)
                        {
                            if (_bitmap.GetPixel(x, y) == Color.FromArgb(255, 0, 0, 0))
                            {
                                sb.Append("A,Wnu,");
                            }
                            sb.Append("DD,Wnu,");
                        }
                    }
                    else
                    {
                        for (int y = _bitmap.Height - 1; y >= 0; y--)
                        {
                            if (_bitmap.GetPixel(x, y) == Color.FromArgb(255, 0, 0, 0))
                            {
                                sb.Append("A,Wnu,");
                            }
                            sb.Append("DU,Wnu,");
                        }
                    }

                    currentDirection = !currentDirection;
                }

                sb.Append("DR,Wnu,");
                verticalLength += sb.Length;
                verticalOutput[x] = sb.ToString();
                sb.Clear();
            }

            // Get horizontal path
            currentDirection = true;

            for (int y = 0; y < _bitmap.Height; y++)
            {
                if (!LineIsEmpty(y, false))
                {
                    if (currentDirection)
                    {
                        for (int x = 0; x < _bitmap.Width; x++)
                        {
                            if (_bitmap.GetPixel(x, y) == Color.FromArgb(255, 0, 0, 0))
                            {
                                sb.Append("A,Wnu,");
                            }
                            sb.Append("DR,Wnu,");

                        }
                    }
                    else
                    {
                        for (int x = _bitmap.Width - 1; x >= 0; x--)
                        {
                            if (_bitmap.GetPixel(x, y) == Color.FromArgb(255, 0, 0, 0))
                            {
                                sb.Append("A,Wnu,");
                            }
                            sb.Append("DL,Wnu,");
                        }
                    }

                    currentDirection = !currentDirection;
                }

                sb.Append("DD,Wnu");
                horizontalLength += sb.Length;
                horizontalOutput[y] = sb.ToString();
                sb.Clear();
            }

            if (horizontalLength > verticalLength)
            {
                maxPointer = 120;
                return horizontalOutput;
            }
            else
            {
                maxPointer = 320;
                return verticalOutput;
            }
        }
    }
}