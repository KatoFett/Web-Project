
(function () {
    const allCategories = [
        "helmet",
        "chestplate",
        "leggings",
        "boots",
        "sword",
        "bow",
        "crossbow",
        "tool",
        "trident",
        "misc"
    ];
    const armorCategories = [
        "helmet",
        "chestplate",
        "leggings",
        "boots"
    ];
    const data = {
        "enchantments": [
            {
                "id": "mending",
                "shortDescription": "Repairs with XP.",
                "longDescription": "Whenever a player collects an XP orb, there is a chance it instead repairs the item for a small amount. XP collected can either repair the item or go into the player's XP bar, but not both. "
                    + "This enchantment is arguably the most valuable in the game because of its ability to keep gear forever.",
                "maxLevel": 1,
                "exclusions": [
                    "infinity"
                ],
                "categories": allCategories,

            },
            {
                "id": "unbreaking",
                "shortDescription": "Increased durability.",
                "longDescription": "Whenever the item would be damaged, there is a chance that it won't. This enchantment is frequently coupled with the $mending$ enchantment.",
                "levelDescription": "The chance increases with each level.",
                "maxLevel": 3,
                "categories": allCategories
            },
            {
                "id": "protection",
                "shortDescription": "Medium protection against everything.",
                "longDescription": "Provides medium protection against all damage sources except the void and /kill command. This enchantment is usually the go-to for armor. "
                    + "It is negated by the $sharpness$ enchantment.",
                "levelDescription": "Protection amount increases with each level.",
                "maxLevel": 4,
                "exclusions": [
                    "blast-protection",
                    "fire-protection",
                    "projectile-protection"
                ],
                "categories": armorCategories
            },
            {
                "id": "blast-protection",
                "shortDescription": "Large protection against explosions.",
                "longDescription": "Provides large protection against explosive damage. This enchantment is better than $protection$ when explosions will be frequent.",
                "levelDescription": "Protection amount increases with each level.",
                "maxLevel": 4,
                "exclusions": [
                    "protection",
                    "fire-protection",
                    "projectile-protection"
                ],
                "categories": armorCategories
            },
            {
                "id": "fire-protection",
                "shortDescription": "Large protection against fire.",
                "longDescription": "Provides large protection against fire and lava damage.",
                "levelDescription": "Protection amount increases with each level.",
                "maxLevel": 4,
                "exclusions": [
                    "protection",
                    "blast-protection",
                    "projectile-protection"
                ],
                "categories": armorCategories
            },
            {
                "id": "projectile-protection",
                "shortDescription": "Large protection against projectiles.",
                "longDescription": "Provides large protection against projectile damage (e.g. arrows, fireballs, tridents).",
                "levelDescription": "Protection amount increases with each level.",
                "maxLevel": 4,
                "exclusions": [
                    "protection",
                    "blast-protection",
                    "fire-protection"
                ],
                "categories": armorCategories
            },
            {
                "id": "thorns",
                "shortDescription": "Damages attackers.",
                "longDescription": "Whenever an entity attacks the wearer, that entity will automatically take counter damage. The chance of a counterattack increases with each piece of armor that is enchanted with thorns. "
                    + "If all four armor pieces have thorns, then every attack will be counterattacked.",
                "levelDescription": "Counterattack damage increases with each level.",
                "maxLevel": 3,
                "categories": armorCategories
            },
            {
                "id": "aqua-affinity",
                "shortDescription": "Faster underwater mining.",
                "longDescription": "Removes the dig speed cost when underwater. In other words, mining underwater will be just as fast as mining on land.",
                "maxLevel": 1,
                "categories": ["helmet"]
            },
            {
                "id": "respiration",
                "shortDescription": "Breathe underwater for longer.",
                "longDescription": "Air bubbles will take longer to disappear while underwater.",
                "levelDescription": "Breath duration increases with each level.",
                "maxLevel": 3,
                "categories": ["helmet"]
            },
            {
                "id": "feather-falling",
                "shortDescription": "Reduced fall damage.",
                "longDescription": "Reduces fall damage. This enchantment is highly recommended for survivability.",
                "levelDescription": "Reduction amount increases with each level.",
                "maxLevel": 4,
                "categories": ["boots"]
            },
            {
                "id": "depth-strider",
                "shortDescription": "Swim faster.",
                "longDescription": "Movement speed is increased when in water. Only the boots need to be submerged in order to gain the speed buff.",
                "levelDescription": "Speed increases with each level.",
                "maxLevel": 3,
                "categories": ["boots"],
                "exclusions": [
                    "frost-walker",
                    "soul-speed"
                ]
            },
            {
                "id": "frost-walker",
                "shortDescription": "Creates ice when you walk.",
                "longDescription": "You will freeze nearby water when walking, creating a path. Unless in a cold biome, the ice will eventually thaw.",
                "levelDescription": "Freeze radius increases with each level.",
                "maxLevel": 2,
                "categories": ["boots"],
                "exclusions": [
                    "depth-strider",
                    "soul-speed"
                ]
            },
            {
                "id": "soul-speed",
                "shortDescription": "Run faster on soul sand.",
                "longDescription": "Movement speed is increased while on soul sand. It is possible to run faster on soul sand than normal blocks.",
                "levelDescription": "Speed increases with each level.",
                "maxLevel": 3,
                "categories": ["boots"],
                "exclusions": [
                    "depth-strider",
                    "soul-speed"
                ]
            },
            {
                "id": "sharpness",
                "shortDescription": "Medium damage against all entities.",
                "longDescription": "Increases damage by a medium amount against all entities. This enchantment is usually the go-to for swords and/or axes. "
                    + "It is negated by the $protection$ enchantment.",
                "levelDescription": "Damage increases with each level.",
                "maxLevel": 5,
                "categories": [
                    "sword",
                    "tool"
                ],
                "exclusions": [
                    "bane-of-arthropods",
                    "smite"
                ]
            },
            {
                "id": "bane-of-arthropods",
                "shortDescription": "Large damage against arthropods.",
                "longDescription": "Increases damage by a large amount against arthropods (silverfish, spiders, etc). This enchantment is generally not recommended as arthropods already have low health.",
                "levelDescription": "Damage increases with each level.",
                "maxLevel": 5,
                "categories": ["sword"],
                "exclusions": [
                    "sharpness",
                    "smite"
                ]
            },
            {
                "id": "smite",
                "shortDescription": "Large damage against undead.",
                "longDescription": "Increases damage by a large amount against undead (zombies, skeletons, etc).",
                "levelDescription": "Damage increases with each level.",
                "maxLevel": 5,
                "categories": ["sword"],
                "exclusions": [
                    "sharpness",
                    "bane-of-arthropods"
                ]
            },
            {
                "id": "knockback",
                "shortDescription": "Knock enemies backwards.",
                "longDescription": "Attacking an entity will send it away from you. It is negated by the enemy's passive knockback resistance which can "
                    + "also be increased by wearing Netherite armor. For example, iron golems will not take knockback, even after being hit by a knockback 2 sword.",
                "levelDescription": "Distance increases with each level.",
                "maxLevel": 2,
                "categories": ["sword"],
            },
            {
                "id": "fire-aspect",
                "shortDescription": "Ignite enemies.",
                "longDescription": "Attacking an entity will light it on fire. Unless it dies instantly, meat dropped by passive mobs will be cooked.",
                "levelDescription": "Fire duration increases with each level.",
                "maxLevel": 2,
                "categories": ["sword"],
            },
            {
                "id": "sweeping-edge",
                "shortDescription": "Sweep attacks are deadlier.",
                "longDescription": "Damage done to other mobs hit by the sweep attack will be increased but will never be more than the sword can deal.",
                "levelDescription": "Damage increases with each level.",
                "maxLevel": 3,
                "categories": ["sword"],
            },
            {
                "id": "looting",
                "shortDescription": "More mob drops.",
                "longDescription": "Increases the chance and quantity of loot from killing mobs. For example, sheep will drop more mutton and wither skeletons will drop their skull more frequently. The loot depends on each mob.",
                "levelDescription": "Loot increases with each level.",
                "maxLevel": 3,
                "categories": ["sword"],
            },
            {
                "id": "power",
                "shortDescription": "Increased arrow damage.",
                "longDescription": "Increases damage done by arrows shot by the bow.",
                "levelDescription": "Damage increases with each level.",
                "maxLevel": 5,
                "categories": ["bow"]
            },
            {
                "id": "punch",
                "shortDescription": "Knock enemies backwards.",
                "longDescription": "Shooting an entity will send it flying away from you.",
                "levelDescription": "Distance increases with each level.",
                "maxLevel": 2,
                "categories": ["bow"]
            },
            {
                "id": "flame",
                "shortDescription": "Ignite enemies.",
                "longDescription": "Shooting an entity will light it on fire. Unless it dies instantly, meat dropped by passive mobs will be cooked.",
                "levelDescription": "Fire duration increases with each level.",
                "maxLevel": 2,
                "categories": ["bow"]
            },
            {
                "id": "infinity",
                "shortDescription": "Infinite arrows.",
                "longDescription": "Shooting will not consume arrows. Only regular arrows are affected by the Infinity enchantment; tipped arrows will be consumed normally.",
                "maxLevel": 1,
                "categories": ["bow"],
                "exclusions": [
                    "mending"
                ]
            }
        ],
        "categories": [
            {
                "id": "helmet",
                "recommended": [
                    "mending",
                    "unbreaking",
                    "protection",
                    "thorns",
                    "aqua-affinity",
                    "respiration"
                ],
                "recommendDescription": "<ul>"
                    + "<li>$mending$ and $unbreaking$ are practically necessary if you want to keep the helmet forever.</li>"
                    + "<li>$protection$ is chosen as it protects from pretty much all damage. However, this can be swapped with any of the protection enchantments depending on the situation.</li>"
                    + "<li>Except in PvP, $thorns$ is not necessary. It helps fend off zombies that sneak up on you, but not much more.</li>"
                    + "<li>$aqua-affinity$ and $respiration$ can be a life-saver when underwater.</li>"
                    + "</ul>"
            },
            {
                "id": "chestplate",
                "recommended": [
                    "mending",
                    "unbreaking",
                    "protection",
                    "thorns"
                ],
                "recommendDescription": "<ul>"
                    + "<li>$mending$ and $unbreaking$ are practically necessary if you want to keep the chestplate forever.</li>"
                    + "<li>$protection$ is chosen as it protects from pretty much all damage. However, this can be swapped with any of the protection enchantments depending on the situation.</li>"
                    + "<li>Except in PvP, $thorns$ is not necessary. It helps fend off zombies that sneak up on you, but not much more.</li>"
                    + "</ul>"
            },
            {
                "id": "leggings",
                "recommended": [
                    "mending",
                    "unbreaking",
                    "protection",
                    "thorns"
                ],
                "recommendDescription": "<ul>"
                    + "<li>$mending$ and $unbreaking$ are practically necessary if you want to keep the leggings forever.</li>"
                    + "<li>$protection$ is chosen as it protects from pretty much all damage. However, this can be swapped with any of the protection enchantments depending on the situation.</li>"
                    + "<li>Except in PvP, $thorns$ is not necessary. It helps fend off zombies that sneak up on you, but not much more.</li>"
                    + "</ul>"
            },
            {
                "id": "boots",
                "recommended": [
                    "mending",
                    "unbreaking",
                    "protection",
                    "thorns",
                    "feather-falling",
                    "depth-strider"
                ],
                "recommendDescription": "<ul>"
                    + "<li>$mending$ and $unbreaking$ are practically necessary if you want to keep the boots forever.</li>"
                    + "<li>$protection$ is chosen as it protects from pretty much all damage. However, this can be swapped with any of the protection enchantments depending on the situation.</li>"
                    + "<li>Except in PvP, $thorns$ is not necessary. It helps fend off zombies that sneak up on you, but not much more.</li>"
                    + "<li>$feather-falling$ is a must.</li>"
                    + "<li>$depth-strider$ is up to personal preference. I've found that I'm underwater more often than on soul sand, and freezing water can be annoying when you're on a farm.</li>"
                    + "</ul>"
            }

        ]
    }
    // Load categories

    const categoryLists = document.getElementsByClassName('all-categories');
    var categoryHtml = GetCategoryHTML(data.categories);
    for (const category of categoryLists) {
        category.innerHTML = categoryHtml;
    }

    // Load enchantments

    const enchantmentLists = document.getElementsByClassName('all-enchantments');
    const enchantmentHtml = GetEnchantmentHTML(data.enchantments);
    for (const enchantment of enchantmentLists) {
        enchantment.innerHTML = enchantmentHtml;
    }

    window.data = data;
})();

function GetName(id) {
    var split = id.split('-');
    for (var i = 0; i < split.length; i++) {
        split[i] = split[i][0].toUpperCase() + split[i].slice(1);
    }
    return split.join(' ');
}

function GetEnchantmentHTML(enchantments) {
    var enchantmentHtml = '';
    enchantments.forEach(enchantment => enchantmentHtml += '<li><a href="enchantment.html?selected=' + enchantment.id + '"><img src="images/enchanted_book.gif" /><div><p>'
        + GetName(enchantment.id) + '</p><span>' + enchantment.shortDescription + '</span></div></li></a>');
    return enchantmentHtml;
}

function GetCategoryHTML(categories) {
    var categoryHtml = '';
    categories.forEach(category => categoryHtml += '<li><a href="category.html?selected=' + category.id + '"><img src="images/category_' + category.id + '.png" /><p>' + GetName(category.id) + '</p></li></a>');
    return categoryHtml;
}

function FormatHTML(str) {
    const reg = /\$.*?\$/g;
    const enchants = [...str.matchAll(reg)];
    enchants.forEach(enchant => str = str.replaceAll(enchant[0], '<a href="enchantment?selected=' + enchant[0].replaceAll('$', '') + '">' + GetName(enchant[0].replaceAll('$', '')) + '</a>'));
    return str;
}