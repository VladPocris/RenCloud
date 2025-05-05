using RenCloud;
using System.Drawing;
using System.Windows.Forms;
using static RenCloud.Program;

namespace ProgramTests
{
    [TestClass]
    public class RoundCornersCollorAttribute
    {

        public Form testingForm = null!;

        [DoNotParallelize]
        [TestMethod]
        public void AttributesRoundCorners_ActivatedColor()
        {
            testingForm = new Form();

            var handle = testingForm.Handle;

            Program.IsTesting = true;
            Program.Corners.AttributesRoundCorners(testingForm, true);

            Assert.AreEqual(Program.BordersColor, Color.FromArgb(255, 153, 164), "Expected active border color.");
        }

        [DoNotParallelize]
        [TestMethod]
        public void AttributesRoundCorners_DeactivatedColor()
        {
            testingForm = new Form();

            var handle = testingForm.Handle;

            Program.IsTesting = true;
            Program.Corners.AttributesRoundCorners(testingForm, false);

            Assert.AreEqual(Program.BordersColor, Color.FromArgb(89, 76, 255), "Expected non-active border color.");
        }
    }

    [TestClass]
    public class GifAnimationTests
    {

        public Form testingForm = null!;

        //Sample gif animation for testing using AimatedGif package (stack overflow sample)
        private (Bitmap gif, string tempPath) GetTestAnimatedGif()
        {
            string tempPath = Path.GetTempFileName();

            using (var gif = AnimatedGif.AnimatedGif.Create(tempPath, 100))
            {
                for (int i = 0; i < 2; i++)
                {
                    using (var frame = new Bitmap(10, 10))
                    using (var g = Graphics.FromImage(frame))
                    {
                        g.Clear(i % 2 == 0 ? Color.Red : Color.Blue);
                        gif.AddFrame(frame, delay: 100);
                    }
                }
            }

            byte[] gifBytes = File.ReadAllBytes(tempPath);
            var stream = new MemoryStream(gifBytes);
            Bitmap gifBitmap = (Bitmap)Image.FromStream(stream);

            return (gifBitmap, tempPath);
        }



        [TestMethod]
        public void InitializeGifAnimation_Reinitializes_DisposesPreviousResources()
        {
            var gifControl = new GifAnimation(new PictureBox());
            var dummyGif1 = new Bitmap(10, 10);
            var dummyGif2 = new Bitmap(10, 10);

            gifControl.InitializeGifAnimation(dummyGif1);
            gifControl.InitializeGifAnimation(dummyGif2);

            Assert.IsNotNull(dummyGif2, "Gif null.");
        }

        [TestMethod]
        public void OnFrameChanged_CallsTickUpdate()
        {
            var pictureBox = new PictureBox();
            var gifControl = new GifAnimation(pictureBox);
            var dummyGif = new Bitmap(10, 10);

            gifControl.InitializeGifAnimation(dummyGif);
            gifControl.OnFrameChanged(dummyGif, EventArgs.Empty);

            Assert.IsTrue(Program.TickUpdateCalled, "TickUpdate was not triggered by OnFrameChanged.");
        }

        [TestMethod]
        public void FrameUpdate_WithAnimatedImage_ExceptionNotThrown()
        {
            var pictureBox = new PictureBox
            {
                Width = 50,
                Height = 50
            };

            GifAnimation gifControl = new GifAnimation(pictureBox);
            Bitmap dummyGif = new Bitmap(10, 10);
            gifControl.InitializeGifAnimation(dummyGif);

            using Bitmap btmp = new Bitmap(50, 50);
            using Graphics g = Graphics.FromImage(btmp);
            PaintEventArgs paintEvent = new PaintEventArgs(g, new Rectangle(0, 0, 50, 50));

            try
            {
                gifControl.FrameUpdate(null!, paintEvent);
            }
            catch (Exception ex)
            {
                Assert.Fail($"FrameUpdate threw an exception: {ex.Message}");
            }
        }

        [TestMethod]
        public void TickUpdate_AdvancesFrame()
        {
            var pictureBox = new PictureBox();
            var gifControl = new GifAnimation(pictureBox);

            var (animatedGif, tempPath) = GetTestAnimatedGif();

            try
            {
                gifControl.InitializeGifAnimation(animatedGif);

                int initialFrame = gifControl.currentFrame;

                gifControl.TickUpdate(pictureBox);

                int newFrame = gifControl.currentFrame;
                Assert.AreNotEqual(initialFrame, newFrame, "Frame did not move to next frame as expected.");
            }
            finally
            {
                if (File.Exists(tempPath))
                    File.Delete(tempPath);
            }
        }
    }

    [TestClass]
    public class DraggingTests
    {
        [TestMethod]
        public void InitializationOfDragging_DoesntThrowException()
        {
            var form = new Form();
            var mouseEvent = new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0);

            var handle = form.Handle;

            try
            {
                DragFunctionality.InitializeDragging(mouseEvent, form);
            }
            catch (Exception ex)
            {
                Assert.Fail($"InitializeDragging threw an exception: {ex.Message}");
            }
        }

        [TestMethod]
        public void OnMouseDown_SetsDraggingTrue()
        {
            var form = new Form { Location = new Point(50, 50) };
            var handler = new DragFunctionality();
            var e = new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0);

            handler.OnMouseDown(e, form);

            Assert.IsTrue(handler.IsDragging, "Dragging status was not update (Dragging not started or mouse down event didn't happen.)");
        }

        [TestMethod]
        public void OnMouseMove_UpdatesFormLocation()
        {
            var form = new Form { Location = new Point(100, 100) };
            var handler = new DragFunctionality();

            handler.IsDragging = true;
            handler.DragCursorPoint = new Point(50, 50);
            handler.DragFormPoint = new Point(100, 100);
            Cursor.Position = new Point(60, 60);

            handler.OnMouseMove(form);

            Assert.AreEqual(new Point(110, 110), form.Location, "Forms location is not where it is expected to be, please check MouseMove event.");
        }

        [TestMethod]
        public void OnMouseUp_SetsDraggingFalse()
        {
            var handler = new DragFunctionality();
            handler.IsDragging = true;
            var e = new MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0);

            handler.OnMouseUp(e);

            Assert.IsFalse(handler.IsDragging, "Mouse up event did not trigger.");
        }
    }

    [TestClass]
    public class RegistrationValidationTests
    {
        [TestMethod]
        public void IsValidRegistration_AllValid()
        {
            var pictureBoxes = new[]
            {
                new PictureBox { Tag = "Valid" },
                new PictureBox { Tag = "Valid" },
                new PictureBox { Tag = "Valid" }
            };

            bool result = RegistrationValid.IsValidRegistration(pictureBoxes);

            Assert.IsTrue(result, "Expected true when all tags are 'Valid'.");
        }

        [TestMethod]
        public void IsValidRegistration_OneInvalid()
        {
            var pictureBoxes = new[]
            {
                new PictureBox { Tag = "Valid" },
                new PictureBox { Tag = "Invalid" },
                new PictureBox { Tag = "Valid" }
            };

            bool result = RegistrationValid.IsValidRegistration(pictureBoxes);

            Assert.IsFalse(result, "Expected false when one tag is not 'Valid'.");
        }

        [TestMethod]
        public void IsValidRegistration_OneNull()
        {
            var pictureBoxes = new[]
            {
                new PictureBox { Tag = "Valid" },
                new PictureBox { Tag = null },
                new PictureBox { Tag = "Valid" }
            };

            bool result = RegistrationValid.IsValidRegistration(pictureBoxes);

            Assert.IsFalse(result, "Expected false when one tag is null.");
        }
    }
}
