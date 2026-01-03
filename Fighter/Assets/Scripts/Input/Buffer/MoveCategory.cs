public enum MoveCategory
{
    Movement,      // Walk, dash, jump
    Normal,        // 5L, 2M, 5H, etc.
    CommandNormal, // 6H, 3M, etc.
    Special,       // 236L, 623H, etc.
    Super,         // 236236H, 214214L, etc.
    Throw,         // Throws
    Universal      // System mechanics
}