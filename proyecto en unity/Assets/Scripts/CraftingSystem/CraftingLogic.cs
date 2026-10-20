using System.Collections.Generic;

// Resultado posible de un intento de crafteo.
public enum CraftResult { Success, WrongItem, WrongRecipe }

// Reglas del crafteo. No toca la UI ni modifica el inventario, solo decide.
public static class CraftingLogic
{
    // 'placed' son los índices de los slots del inventario que el jugador puso en el círculo.
    public static CraftResult Evaluate(IReadOnlyList<int> placed, InventorySlot[] slots, List<Cauldron.Ingredient> recipe)
    {
        // Cualquier stack con unidades incorrectas cancela el crafteo.
        foreach (int index in placed)
        {
            if (slots[index].wrongQuantity > 0) return CraftResult.WrongItem;
        }

        // Tiene que haber exactamente un stack por ingrediente.
        if (placed.Count != recipe.Count) return CraftResult.WrongRecipe;

        foreach (Cauldron.Ingredient ingredient in recipe)
        {
            int matches = 0;
            foreach (int index in placed)
            {
                InventorySlot slot = slots[index];
                if (slot.itemData != ingredient.item) continue;

                matches++;
                if (slot.quantity < ingredient.amount) return CraftResult.WrongRecipe; // Faltan unidades.
            }

            // 0 es que falta el ingrediente. 2 o más es que hay stacks repetidos del mismo item.
            if (matches != 1) return CraftResult.WrongRecipe;
        }

        return CraftResult.Success;
    }
}