using System;
using GameCreator.Runtime.Common;

namespace GameCreator.Editor.Overlays
{
    public abstract class TToolbar : Attribute
    {
        protected const ColorTheme.Type COLOR = ColorTheme.Type.TextLight;

        // MEMBERS: -------------------------------------------------------------------------------
        
        private IIcon m_Icon;

        // PROPERTIES: ----------------------------------------------------------------------------
        
        public IIcon Icon => this.m_Icon ??= this.CreateIcon;

        public abstract string Tooltip { get; }
        public abstract int Priority { get; }

        protected abstract IIcon CreateIcon { get; }

        // METHODS: -------------------------------------------------------------------------------

        public abstract void Run();
    }
}