(function () {
    const urlParams = new window.URLSearchParams(window.location.search);
    const selected = urlParams.get('selected');
    const selectedCategory = window.data.categories.filter(c => c.id == selected)[0];
    document.getElementById('CategoryIcon').src = 'images/category_' + selected + '.png';
    document.getElementById('CategoryName').innerText = GetName(selected);
    const enchantments = new Array();
    window.data.enchantments.forEach(enchantment => {
        if (enchantment.categories.includes(selected)) {
            enchantments.push(enchantment);
        }
    });
    document.getElementById('AllEnchantmentsList').innerHTML = GetEnchantmentHTML(enchantments);

    const recommendedEnchantments = new Array();
    selectedCategory.recommended.forEach(enchantment => {
        recommendedEnchantments.push(enchantments.filter(e => e.id == enchantment)[0]);
    });
    document.getElementById('RecommendedEnchantmentsList').innerHTML = GetEnchantmentHTML(recommendedEnchantments);
    document.getElementById('RecommendationParagraph').innerHTML = FormatHTML(selectedCategory.recommendDescription);
})();