using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName ="Extended Tile Rule",menuName ="2D/Tiles/Extended Rule Tile")]
public class ExtendedRuleTile : RuleTile
{
    public string type;
    public override bool RuleMatch(int neighbor, TileBase other)
    {
        if (other is RuleOverrideTile)
            other = (other as RuleOverrideTile).m_InstanceTile;

        ExtendedRuleTile otherTile = other as ExtendedRuleTile;
        if (other == null || otherTile == null)
            return base.RuleMatch(neighbor, other);
        switch(neighbor)
        {
            case TilingRule.Neighbor.This: return type == otherTile.type;
            case TilingRule.Neighbor.NotThis: return type == otherTile.type;
        }
        return true;
    }
}
