
(function () {
    const enchantments = {
        "enchantments": {
            "mending": {
                "shortDescription": "Repairs with XP.",
                "longDescription": "Whenever a player collects an XP orb, there is a chance it instead repairs this piece of gear for a small amount.",
                "maxLevel": 1,
                "exclusions": [
                    "infinity"
                ],
                "categories": [
                    "helmet",
                    "chest",
                    "leggings",
                    "boots"
                ]
            },
            "unbreaking": {
                "shortDescription": "Increases durability.",
                "longDescription": "Whenever the gear would be damaged, there is a chance that it won't. The chance increases with each level.",
                "maxLevel": 3,
                "categories": [
                    "helmet",
                    "chest",
                    "leggings",
                    "boots"
                ]
            },
            "protection": {
                "shortDescription": "Medium protection against everything.",
                "longDescription": "Provides medium protection against all damage sources except the void and /kill command. Protection amount increases with each level.",
                "maxLevel": 4,
                "exclusions": [
                    "blast-protection",
                    "fire-protection",
                    "projectile-protection"
                ],
                "categories": [
                    "helmet",
                    "chest",
                    "leggings",
                    "boots"
                ]
            },
            "blast-protection": {
                "shortDescription": "Large protection against explosions.",
                "longDescription": "Provides large protection against explosive damage. Protection amount increases with each level.",
                "maxLevel": 4,
                "exclusions": [
                    "protection",
                    "fire-protection",
                    "projectile-protection"
                ],
                "categories": [
                    "helmet",
                    "chest",
                    "leggings",
                    "boots"
                ]
            },
            "fire-protection": {
                "shortDescription": "Large protection against fire.",
                "longDescription": "Provides large protection against fire and lava damage. Protection amount increases with each level.",
                "maxLevel": 4,
                "exclusions": [
                    "protection",
                    "blast-protection",
                    "projectile-protection"
                ],
                "categories": [
                    "helmet",
                    "chest",
                    "leggings",
                    "boots"
                ]
            }
        },
        "categories": [
            "helmet",
            "chest",
            "leggings",
            "boots"
        ]
    }
    // Load categories

    const categoryLists = document.getElementsByClassName('all-categories');
    var categoryHtml = '';
    enchantments.categories.forEach(category => categoryHtml += '<li><a href="category.html?selected=' + category + '"><img src="images/category_' + category + '.png" /><p>' + GetName(category) + '</p></li></a>')
    for (const category of categoryLists) {
        category.innerHTML = categoryHtml;
    }

    // Load enchantments

    const enchantmentLists = document.getElementsByClassName('all-enchantments');
    var enchantmentHtml = '';
    Object.keys(enchantments.enchantments).forEach(enchantment => enchantmentHtml += '<li><a href="enchantment.html?selected=' + enchantment + '"><img src="images/enchanted_book.gif" /><p>' + GetName(enchantment) + '</p></li></a>')
    for (const enchantment of enchantmentLists) {
        enchantment.innerHTML = enchantmentHtml;
    }


})();

function GetName(id) {
    var split = id.split('-');
    for (var i = 0; i < split.length; i++) {
        split[i] = split[i][0].toUpperCase() + split[i].slice(1);
    }
    return split.join(' ');
}