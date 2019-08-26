﻿using System;
using System.Text;

namespace ESCPOS
{
    public static class Commands
    {
        #region Simple Commands
        /// <summary>
        /// Moves the print position to the next tab position.
        /// </summary>
        /// <remarks>
        /// ·This command is ignored unless the next tab position has been set.
        /// ·If the next horizontal tab position exceeds the printing area, the printer sets the printing position to[Printing area width + 1].
        /// ·The default setting of the horizontal tab position for the paper roll is font A (12 x 24) every 8th character(9th, 17th, 25th, … column).
        public static byte[] HorizontalTab => new byte[] { 0x09 };

        /// <summary>
        ///Prints the data in the print buffer and feeds one line based on the current line spacing.
        /// <remarks>
        /// ·This command sets the print position to the beginning of the line.
        public static byte[] LineFeed => new byte[] { 0x0A };

        /// <summary>
        ///When automatic line feed is enabled, this command functions the same as LF, when automatic line feed is disabled, this command is ignored
        /// <remarks>
        /// ·Sets the print starting position to the beginning of the line.
        /// ·The automatic line feed is ignored.
        public static byte[] CarriageReturn => new byte[] { 0x0D };

        /// <summary>
        ///Prints the data in the print buffer and returns to standard mode.
        /// <remarks>
        ///·The buffer data is deleted after being printed.
        ///·The printer does not execute paper cutting.
        ///·This command sets the print position to the beginning of the line.
        ///·This command is enabled only in page mode.
        public static byte[] PrintAndReturnToStandardMode => new byte[] { 0x0C };

        /// <summary>
        ///In page mode, delete all the print data in the current printable area.
        /// <remarks>
        ///·This command is enabled only in page mode.
        ///·If data that existed in the previously specified printable area also exists in the currently specified printable area, it is deleted.
        public static byte[] CancelPrint => new byte[] { 0x18 };

        //Alias
        public static byte[] HT => HorizontalTab;
        public static byte[] LF => LineFeed;
        public static byte[] CR => CarriageReturn;
        public static byte[] FF => PrintAndReturnToStandardMode;
        public static byte[] CAN => CancelPrint;
        #endregion Simple Commands

        /// <summary>
        /// ESC @
        /// </summary>
        public static byte[] InitializePrinter => new byte[] { 0x1B, 0x40 };

        /// <summary>
        /// ESC p m t1 t2
        /// </summary>
        public static byte[] OpenDrawer => new byte[] { 0x1B, 0x70, 0x00, 0x3C, 0x78 };

        /// <summary>
        /// ESC m
        /// </summary>
        public static byte[] PaperCut => new byte[] { 0x1B, 0x6D };

        /// <summary>
        /// ESC i
        /// </summary>
        public static byte[] FullPaperCut => new byte[] { 0x1B, 0x69 };

        /// <summary>
        /// ESC ! n
        /// </summary>
        public static byte[] SelectPrintMode(PrintMode printMode)
            => new byte[] { 0x1B, 0x21, (byte)printMode };

        /// <summary>
        /// ESC - n
        /// </summary>
        public static byte[] SelectUnderlineMode(UnderlineMode underlineMode)
            => new byte[] { 0x1B, 0x2D, (byte)underlineMode };

        /// <summary>
        /// ESC 2 / ESC 3 n
        /// </summary>
        public static byte[] SelectLineSpacing(LineSpacing lineSpacing)
            => lineSpacing == LineSpacing.Default ? new byte[] { 0x1B, (byte)lineSpacing } : new byte[] { 0x1B, 0x33, (byte)lineSpacing };

        /// <summary>
        /// ESC G n
        /// </summary>
        public static byte[] DoubleStrike(OnOff @switch)
            => new byte[] { 0x1B, 0x47, (byte)@switch };

        /// <summary>
        /// ESC M n
        /// </summary>
        public static byte[] SelectCharacterFont(Font font)
            => new byte[] { 0x1B, 0x4D, (byte)font };

        /// <summary>
        /// ESC R n
        /// </summary>
        public static byte[] SelectInternationalCharacterSet(CharSet charSet)
            => new byte[] { 0x1B, 0x52, (byte)charSet };

        /// <summary>
        /// ESC T n
        /// </summary>
        public static byte[] SelectPrintDirection(Direction direction)
            => new byte[] { 0x1B, 0x54, (byte)direction };

        /// <summary>
        /// ESC V n
        /// </summary>
        public static byte[] Turn90Degrees(ClockwiseDirection clockwiseDirection)
            => new byte[] { 0x1B, 0x56, (byte)clockwiseDirection };

        /// <summary>
        /// ESC a n
        /// </summary>
        public static byte[] SelectJustification(Justification justification)
            => new byte[] { 0x1B, 0x61, (byte)justification };

        /// <summary>
        /// ESC t n
        /// </summary>
        public static byte[] SelectCodeTable(CodeTable codeTable)
            => new byte[] { 0x1B, 0x74, (byte)codeTable };

        /// <summary>
        /// ESC { n
        /// </summary>
        public static byte[] UpsideDown(OnOff onOff)
            => new byte[] { 0x1B, 0x7B, (byte)onOff };

        /// <summary>
        /// GS ! n
        /// </summary>
        public static byte[] SelectCharSizeHeight(CharSizeHeight charSize)
            => new byte[] { 0x1D, 0x21, (byte)charSize };

        /// <summary>
        /// GS ! n
        /// </summary>
        public static byte[] SelectCharSizeWidth(CharSizeWidth charSize)
            => new byte[] { 0x1D, 0x21, (byte)charSize };

        /// <summary>
        /// GS H n
        /// </summary>
        public static byte[] SelectHRIPosition(HRIPosition hriPosition)
            => new byte[] { 0x1D, 0x21, (byte)hriPosition };

        /// <summary>
        /// GS k m n
        /// </summary>
        public static byte[] PrintBarCode(BarCodeType barCodeType, string barCode, int heightInDots = 162)
        {
            var height = new byte[] { 0x1D, 0x68, (byte)heightInDots };
            var settings = new byte[] { 0x1D, 0x6B, (byte)barCodeType, (byte)barCode.Length };
            var bar = Encoding.UTF8.GetBytes(barCode);
            return height.Add(settings).Add(bar);
        }

        /// <summary>
        /// GS ( k pL pH cn fn n1 n2
        /// </summary>
        public static byte[] PrintQRCode(string content, QRCodeModel qrCodemodel, QRCodeCorrection qrodeCorrection, QRCodeSize qrCodeSize)
        {
            var model = new byte[] { 0x1D, 0x28, 0x6B, 0x04, 0x00, 0x31, 0x41, (byte)qrCodemodel, 0x00 };
            var size = new byte[] { 0x1D, 0x28, 0x6B, 0x03, 0x00, 0x31, 0x43, (byte)qrCodeSize };
            var errorCorrection = new byte[] { 0x1D, 0x28, 0x6B, 0x03, 0x00, 0x31, 0x45, (byte)qrodeCorrection };
            int num = content.Length + 3;
            int pL = num % 256;
            int pH = num / 256;
            var storeData = new byte[] { 0x1D, 0x28, 0x6B, (byte)pL, (byte)pH, 0x31, 0x50, 0x30 };
            var data = Encoding.UTF8.GetBytes(content);
            var print = new byte[] { 0x1D, 0x28, 0x6B, 0x03, 0x00, 0x31, 0x51, 0x30 };
            return model.Add(size).Add(errorCorrection).Add(storeData).Add(data).Add(print);
        }

    }

}