(function () {
    const urlParams = new window.URLSearchParams(window.location.search);
    const selected = urlParams.get('selected');
    const selectedEnchantment = window.data.enchantments.filter(c => c.id == selected)[0];
    document.getElementById('EnchantmentLvl').innerText = 'Max Level: ' + GetRomanNumeral(selectedEnchantment.maxLevel) + ' (' + selectedEnchantment.maxLevel + ')';
    document.getElementById('LvlDescription').innerText = selectedEnchantment.levelDescription || '';
    var names = document.getElementsByClassName('EnchantmentName');
    for (var i = 0; i < names.length; i++) {
        names[i].innerText = GetName(selected);
    }
    document.getElementById('EnchantmentDescription').innerHTML = FormatHTML(selectedEnchantment.longDescription);
    const categories = window.data.categories.filter(c => selectedEnchantment.categories.includes(c.id));
    document.getElementById('AllCategoriesList').innerHTML = GetCategoryHTML(categories);
    if (selectedEnchantment.exclusions) {
        var exclusions = window.data.enchantments.filter(e => selectedEnchantment.exclusions.includes(e.id));
        document.getElementById('ExclusionsList').innerHTML = GetEnchantmentHTML(exclusions);
    }
    else {
        document.getElementById('NoExclusionsMsg').hidden = false;
        document.getElementById('ExclusionsList').remove(); 
    }
})();

function GetRomanNumeral(no) {
    switch (no) {
        case 2:
            return "II";
        case 3:
            return "III";
        case 4:
            return "IV";
        case 5:
            return "V";
        default:
            return "I";
    }
}