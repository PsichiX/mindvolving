namespace Mindvolving.Visualization.Renderers
{
    enum BatchItemType : int
    {
        Primitive = 0x1,
        Line = 0x2 | Primitive,
        Triangle = 0x4 | Primitive,
        Sprite = 0x8, 
        SpriteRectangle = 0x10 | Sprite,
        String = 0x20 | Sprite
    }
}