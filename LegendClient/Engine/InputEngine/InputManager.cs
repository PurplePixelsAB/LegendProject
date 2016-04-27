using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using Engine.ScreenEngine;

namespace Engine.InputEngine
{
    public class InputManager
    {
        internal InputManager()
        {

        }

        public Point MousePosition {  get { return this.CurrentMouseState.Position; } }

        private KeyboardState CurrentKeyboardState { get; set; }
        private KeyboardState PreviousKeyboardState { get; set; }

        private MouseState CurrentMouseState { get; set; }
        private MouseState PreviousMouseState { get; set; }

        protected internal void Initialize()
        {

        }
        //protected internal void LoadContent()
        //{
        //    throw new NotImplementedException();
        //}
        //protected internal void UnloadContent()
        //{
        //    throw new NotImplementedException();
        //}
        //protected internal void Draw(GameTime gameTime)
        //{
        //    throw new NotImplementedException();
        //}

        protected internal void Update(GameTime gameTime)
        {
            PreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();

            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();
        }

        //internal bool IsMouseTriggered
        //{
        //    get
        //    {
        //        return ((CurrentMouseState.LeftButton == ButtonState.Released && PreviousMouseState.LeftButton == ButtonState.Pressed) ||
        //        (CurrentMouseState.RightButton == ButtonState.Released && PreviousMouseState.RightButton == ButtonState.Pressed) ||
        //        (CurrentMouseState.MiddleButton == ButtonState.Released && PreviousMouseState.MiddleButton == ButtonState.Pressed) ||
        //        (CurrentMouseState.XButton1 == ButtonState.Released && PreviousMouseState.XButton1 == ButtonState.Pressed) ||
        //        (CurrentMouseState.XButton2 == ButtonState.Released && PreviousMouseState.XButton2 == ButtonState.Pressed));
        //    }
        //}
        //internal bool MouseHasMoved
        //{
        //    get
        //    {
        //        return ((CurrentMouseState.X != PreviousMouseState.X) || (CurrentMouseState.Y != PreviousMouseState.Y));
        //    }
        //}
        //internal bool MousewheelScrolled
        //{
        //    get
        //    {
        //        return (CurrentMouseState.ScrollWheelValue);
        //    }
        //}




        #region Keyboard
        internal bool IsKeyPressed(Keys key)
        {
            return IsKeyPressed(CurrentKeyboardState, key);
        }
        internal bool IsKeyPressed(KeyboardState keyboardState, Keys key)
        {
            if (key == Keys.None)
                return true;

            if (keyboardState == null)
                return false;

            return keyboardState.IsKeyDown(key);
        }
        internal bool IsKeyTriggered(Keys key)
        {
            if (key == Keys.None)
                return true;

            if (CurrentKeyboardState == null || PreviousKeyboardState == null)
                return false;

            return ((CurrentKeyboardState.IsKeyDown(key)) && (!PreviousKeyboardState.IsKeyDown(key)));
        }
        internal bool IsKeyReleased(Keys key)
        {
            if (key == Keys.None)
                return true;

            if (CurrentKeyboardState == null || PreviousKeyboardState == null)
                return false;

            return ((!CurrentKeyboardState.IsKeyDown(key)) && (PreviousKeyboardState.IsKeyDown(key)));
        }

        internal bool IsActionPressed(ActionKeyMapping action)
        {
            if (action.Primary != Keys.None)
            {
                if (IsKeyPressed(action.Primary) && IsKeyPressed(action.PrimaryMod))
                    return true;
            }
            if (action.Secondary != Keys.None)
            {
                if (IsKeyPressed(action.Secondary) && IsKeyPressed(action.SecondaryMod))
                    return true;
            }

            return false;
        }
        internal bool IsActionTriggered(ActionKeyMapping action)
        {
            if (action.Primary != Keys.None)
            {
                if (IsKeyTriggered(action.Primary) && (IsKeyTriggered(action.PrimaryMod) || IsKeyPressed(action.PrimaryMod)))
                    return true;
            }
            if (action.Secondary != Keys.None)
            {
                if (IsKeyTriggered(action.Secondary) && (IsKeyTriggered(action.SecondaryMod) || IsKeyPressed(action.SecondaryMod)))
                    return true;
            }

            return false;
        }
        internal bool IsActionReleased(ActionKeyMapping action)
        {
            if (action.Primary != Keys.None)
            {
                if (IsKeyReleased(action.Primary) && (IsKeyTriggered(action.PrimaryMod) || IsKeyPressed(action.PrimaryMod)))
                    return true;
            }
            if (action.Secondary != Keys.None)
            {
                if (IsKeyReleased(action.Secondary) && (IsKeyTriggered(action.SecondaryMod) || IsKeyPressed(action.SecondaryMod)))
                    return true;
            }

            return false;
        }
        #endregion

        #region MouseButton
        internal bool IsButtonPressed(MouseButtons key)
        {
            if (CurrentMouseState == null)
                return false;

            switch (key)
            {
                case MouseButtons.None:
                    return true;
                case MouseButtons.Left:
                    return CurrentMouseState.LeftButton == ButtonState.Pressed;
                case MouseButtons.Right:
                    return CurrentMouseState.RightButton == ButtonState.Pressed;
                case MouseButtons.Middle:
                    return CurrentMouseState.MiddleButton == ButtonState.Pressed;
                case MouseButtons.X1:
                    return CurrentMouseState.XButton1 == ButtonState.Pressed;
                case MouseButtons.X2:
                    return CurrentMouseState.XButton2 == ButtonState.Pressed;
                default:
                    return false;
            }
        }
        internal bool IsButtonTriggered(MouseButtons key)
        {
            if (CurrentMouseState == null || PreviousMouseState == null)
                return false;

            switch (key)
            {
                case MouseButtons.None:
                    return true;
                case MouseButtons.Left:
                    return CurrentMouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released;
                case MouseButtons.Right:
                    return CurrentMouseState.RightButton == ButtonState.Pressed && PreviousMouseState.RightButton == ButtonState.Released;
                case MouseButtons.Middle:
                    return CurrentMouseState.MiddleButton == ButtonState.Pressed && PreviousMouseState.MiddleButton == ButtonState.Released;
                case MouseButtons.X1:
                    return CurrentMouseState.XButton1 == ButtonState.Pressed && PreviousMouseState.XButton1 == ButtonState.Released;
                case MouseButtons.X2:
                    return CurrentMouseState.XButton2 == ButtonState.Pressed && PreviousMouseState.XButton2 == ButtonState.Released;
                case MouseButtons.WheelUp:
                    return WheelMoveUp();
                case MouseButtons.WheelDown:
                    return WheelMoveDown();
                default:
                    return false;
            }
        }
        internal bool IsActionPressed(ActionButtonMapping action)
        {
            if (action.Primary != MouseButtons.None)
            {
                if (IsButtonPressed(action.Primary) && IsKeyPressed(action.PrimaryMod))
                    return true;
            }
            else if (action.Secondary != MouseButtons.None)
            {
                if (IsButtonPressed(action.Secondary) && IsKeyPressed(action.SecondaryMod))
                    return true;
            }

            return false;
        }
        internal bool IsActionTriggered(ActionButtonMapping action)
        {
            if (action.Primary != MouseButtons.None)
            {
                if (IsButtonTriggered(action.Primary) && (IsKeyTriggered(action.PrimaryMod) || IsKeyPressed(action.PrimaryMod)))
                    return true;
            }
            if (action.Secondary != MouseButtons.None)
            {
                if (IsButtonTriggered(action.Secondary) && (IsKeyTriggered(action.SecondaryMod) || IsKeyPressed(action.SecondaryMod)))
                    return true;
            }

            return false;
        }

        private int WheelMoveDelta
        {
            get
            {
                return CurrentMouseState.ScrollWheelValue - PreviousMouseState.ScrollWheelValue;
            }
        }
        private bool WheelMoveUp()
        {
            return WheelMoveDelta > 0;
        }
        private bool WheelMoveDown()
        {
            return WheelMoveDelta < 0;
        }
        #endregion

        #region MouseAxis
        internal bool IsAxisHover(Rectangle position)
        {
            if (CurrentMouseState == null)
                return false;

            Rectangle currentMouseRectangle = new Rectangle(CurrentMouseState.X, CurrentMouseState.Y, 1, 1);
            return position.Intersects(currentMouseRectangle);
        }
        internal bool IsAxisEntering(Rectangle position)
        {
            if (CurrentMouseState == null || PreviousMouseState == null)
                return false;

            Rectangle currentMouseRectangle = new Rectangle(CurrentMouseState.X, CurrentMouseState.Y, 1, 1);
            Rectangle previousMouseRectangle = new Rectangle(PreviousMouseState.X, PreviousMouseState.Y, 1, 1);
            return (position.Intersects(currentMouseRectangle) && !position.Intersects(previousMouseRectangle));
        }
        internal bool IsAxisLeaving(Rectangle position)
        {
            if (CurrentMouseState == null || PreviousMouseState == null)
                return false;

            Rectangle currentMouseRectangle = new Rectangle(CurrentMouseState.X, CurrentMouseState.Y, 1, 1);
            Rectangle previousMouseRectangle = new Rectangle(PreviousMouseState.X, PreviousMouseState.Y, 1, 1);
            return (!position.Intersects(currentMouseRectangle) && position.Intersects(previousMouseRectangle));
        }

        internal bool IsAxisEntering(ActionButtonAxisMapping action)
        {
            if (action.ScreenPosition == Rectangle.Empty)
                return false;

            return IsAxisEntering(action.ScreenPosition);
        }
        internal bool IsAxisLeaving(ActionButtonAxisMapping action)
        {
            if (action.ScreenPosition == Rectangle.Empty)
                return false;

            return IsAxisLeaving(action.ScreenPosition);
        }
        internal bool IsActionPressed(ActionButtonAxisMapping action)
        {
            if (!IsAxisHover(action.ScreenPosition))
                return false;

            if (action.Primary != MouseButtons.None)
            {
                if (IsButtonPressed(action.Primary) && IsKeyPressed(action.PrimaryMod))
                    return true;
            }
            if (action.Secondary != MouseButtons.None)
            {
                if (IsButtonPressed(action.Secondary) && IsKeyPressed(action.SecondaryMod))
                    return true;
            }

            return false;
        }
        internal bool IsActionTriggered(ActionButtonAxisMapping action)
        {
            if (!IsAxisHover(action.ScreenPosition))
                return false;

            if (action.Primary != MouseButtons.None)
            {
                if (IsButtonTriggered(action.Primary) && (IsKeyTriggered(action.PrimaryMod) || IsKeyPressed(action.PrimaryMod)))
                    return true;
            }
            if (action.Secondary != MouseButtons.None)
            {
                if (IsButtonTriggered(action.Secondary) && (IsKeyTriggered(action.SecondaryMod) || IsKeyPressed(action.SecondaryMod)))
                    return true;
            }

            return false;
        } 
        #endregion
    }
}
