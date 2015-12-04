using System;
using System.Collections.Generic;
using System.Linq;

namespace EMRG.Console.Helpers
{
    internal class BodyMetricGenerator
    {
        private const int ChanceOfBMIChange = 15;

        private const double KilogramsPerPound = 0.45359237;
        private const int MaximumDelta = 2;
        private const int MaximumNormalBMI = 29;
        private const int MaximumObeseBMI = 35;
        private const int MinimumNormalBMI = 19;
        private const int MinimumObeseBMI = 30;

        public static BodyMetricGenerator Instance { get; } = new BodyMetricGenerator();

        private List<int> _bmis = new List<int> { 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54 };

        private Dictionary<int, List<int>> _weightInPoundsByHeightInInches = new Dictionary<int, List<int>>
        {
            [58] = new List<int> { 91, 96, 100, 105, 110, 115, 119, 124, 129, 134, 138, 143, 148, 153, 158, 162, 167, 172, 177, 181, 186, 191, 196, 201, 205, 210, 215, 220, 224, 229, 234, 239, 244, 248, 253, 258 },
            [59] = new List<int> { 94, 99, 104, 109, 114, 119, 124, 128, 133, 138, 143, 148, 153, 158, 163, 168, 173, 178, 183, 188, 193, 198, 203, 208, 212, 217, 222, 227, 232, 237, 242, 247, 252, 257, 262, 267 },
            [60] = new List<int> { 97, 102, 107, 112, 118, 123, 128, 133, 138, 143, 148, 153, 158, 163, 168, 174, 179, 184, 189, 194, 199, 204, 209, 215, 220, 225, 230, 235, 240, 245, 250, 255, 261, 266, 271, 276 },
            [61] = new List<int> { 100, 106, 111, 116, 122, 127, 132, 137, 143, 148, 153, 158, 164, 169, 174, 180, 185, 190, 195, 201, 206, 211, 217, 222, 227, 232, 238, 243, 248, 254, 259, 264, 269, 275, 280, 285 },
            [62] = new List<int> { 104, 109, 115, 120, 126, 131, 136, 142, 147, 153, 158, 164, 169, 175, 180, 186, 191, 196, 202, 207, 213, 218, 224, 229, 235, 240, 246, 251, 256, 262, 267, 273, 278, 284, 289, 295 },
            [63] = new List<int> { 107, 113, 118, 124, 130, 135, 141, 146, 152, 158, 163, 169, 175, 180, 186, 191, 197, 203, 208, 214, 220, 225, 231, 237, 242, 248, 254, 259, 265, 270, 278, 282, 287, 293, 299, 304 },
            [64] = new List<int> { 110, 116, 122, 128, 134, 140, 145, 151, 157, 163, 169, 174, 180, 186, 192, 197, 204, 209, 215, 221, 227, 232, 238, 244, 250, 256, 262, 267, 273, 279, 285, 291, 296, 302, 308, 314 },
            [65] = new List<int> { 114, 120, 126, 132, 138, 144, 150, 156, 162, 168, 174, 180, 186, 192, 198, 204, 210, 216, 222, 228, 234, 240, 246, 252, 258, 264, 270, 276, 282, 288, 294, 300, 306, 312, 318, 324 },
            [66] = new List<int> { 118, 124, 130, 136, 142, 148, 155, 161, 167, 173, 179, 186, 192, 198, 204, 210, 216, 223, 229, 235, 241, 247, 253, 260, 266, 272, 278, 284, 291, 297, 303, 309, 315, 322, 328, 334 },
            [67] = new List<int> { 121, 127, 134, 140, 146, 153, 159, 166, 172, 178, 185, 191, 198, 204, 211, 217, 223, 230, 236, 242, 249, 255, 261, 268, 274, 280, 287, 293, 299, 306, 312, 319, 325, 331, 338, 344 },
            [68] = new List<int> { 125, 131, 138, 144, 151, 158, 164, 171, 177, 184, 190, 197, 203, 210, 216, 223, 230, 236, 243, 249, 256, 262, 269, 276, 282, 289, 295, 302, 308, 315, 322, 328, 335, 341, 348, 354 },
            [69] = new List<int> { 128, 135, 142, 149, 155, 162, 169, 176, 182, 189, 196, 203, 209, 216, 223, 230, 236, 243, 250, 257, 263, 270, 277, 284, 291, 297, 304, 311, 318, 324, 331, 338, 345, 351, 358, 365 },
            [70] = new List<int> { 132, 139, 146, 153, 160, 167, 174, 181, 188, 195, 202, 209, 216, 222, 229, 236, 243, 250, 257, 264, 271, 278, 285, 292, 299, 306, 313, 320, 327, 334, 341, 348, 355, 362, 369, 376 },
            [71] = new List<int> { 136, 143, 150, 157, 165, 172, 179, 186, 193, 200, 208, 215, 222, 229, 236, 243, 250, 257, 265, 272, 279, 286, 293, 301, 308, 315, 322, 329, 338, 343, 351, 358, 365, 372, 379, 386 },
            [72] = new List<int> { 140, 147, 154, 162, 169, 177, 184, 191, 199, 206, 213, 221, 228, 235, 242, 250, 258, 265, 272, 279, 287, 294, 302, 309, 316, 324, 331, 338, 346, 353, 361, 368, 375, 383, 390, 397 },
            [73] = new List<int> { 144, 151, 159, 166, 174, 182, 189, 197, 204, 212, 219, 227, 235, 242, 250, 257, 265, 272, 280, 288, 295, 302, 310, 318, 325, 333, 340, 348, 355, 363, 371, 378, 386, 393, 401, 408 },
            [74] = new List<int> { 148, 155, 163, 171, 179, 186, 194, 202, 210, 218, 225, 233, 241, 249, 256, 264, 272, 280, 287, 295, 303, 311, 319, 326, 334, 342, 350, 358, 365, 373, 381, 389, 396, 404, 412, 420 },
            [75] = new List<int> { 152, 160, 168, 176, 184, 192, 200, 208, 216, 224, 232, 240, 248, 256, 264, 272, 279, 287, 295, 303, 311, 319, 327, 335, 343, 351, 359, 367, 375, 383, 391, 399, 407, 415, 423, 431 },
            [76] = new List<int> { 156, 164, 172, 180, 189, 197, 205, 213, 221, 230, 238, 246, 254, 263, 271, 279, 287, 295, 304, 312, 320, 328, 336, 344, 353, 361, 369, 377, 385, 394, 402, 410, 418, 426, 435, 443 }
        };

        public double GetBMI(Random randomizer, bool obese = false, double? baseBMI = null)
        {
            var minimumBMI = obese ? MinimumObeseBMI : MinimumNormalBMI;
            var maximumBMI = obese ? MaximumObeseBMI : MaximumNormalBMI;

            var bmi = baseBMI ?? randomizer.Next(minimumBMI, maximumBMI + 1);
            if (baseBMI.HasValue)
            {
                var hasBMIChanged = randomizer.NextPercent() <= ChanceOfBMIChange;
                if (hasBMIChanged)
                {
                    var direction = randomizer.NextDouble() > 0.5 ? 1 : -1;
                    var delta = randomizer.Next(0, MaximumDelta + 1) * direction;
                    bmi += delta;
                }
            }

            return Math.Max(Math.Min(bmi, maximumBMI), minimumBMI);
        }

        public int GetRandomHeightInInches(Random randomizer)
        {
            return randomizer.NextListElement(_weightInPoundsByHeightInInches.Keys.ToList());
        }

        public double GetWeightInKilograms(double bmi, int heightInInches)
        {
            var bmiIndex = _bmis.IndexOf((int)bmi);
            var weightInPounds = _weightInPoundsByHeightInInches[heightInInches][bmiIndex];
            return Math.Round(weightInPounds * KilogramsPerPound, 2);
        }
    }
}
