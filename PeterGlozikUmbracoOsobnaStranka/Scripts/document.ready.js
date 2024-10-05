$(document).ready(function () {
    // Submenu - handling hover for dropdown
    $('ul.nav li.dropdown').hover(
        function () { $(this).addClass('open'); },
        function () { $(this).removeClass('open'); }
    );

    // Lazy load for images
    $('img.lazy').lazy();

    // Toggle navigation icon
    $('#menuIcon').click(function () {
        $('#menuIcon').toggleClass('fa-bars fa-times');
    });

    $('#menuIconProtect').click(function () {
        $('#menuIconProtect').toggleClass('fa-bars fa-times');
    });










    // CHATBOT

    // Close the chatbot when the close button is clicked
    $('#closeButton').on('click', function () {
        $('#chatbot').hide(); // Hide the chatbot
        $('#openButton').show(); // Show the open button again
        localStorage.setItem('chatbotClosed', 'true'); // Save that chatbot was closed
    });

    // Show the chatbot when the open button is clicked
    $('#openButton').on('click', function () {
        $('#chatbot').show(); // Show the chatbot
        $(this).hide(); // Hide the open button
        startChat(); // Start chat
        localStorage.removeItem('chatbotClosed'); // Remove the closed flag so it can open next time
    });

    // Check if chatbot was previously closed
    if (localStorage.getItem('chatbotClosed') !== 'true') {
        $('#chatbot').show(); // Show the chatbot on page load if it wasn't closed before
        $('#openButton').hide(); // Hide the open button
        startChat(); // Start chat
    } else {
        $('#chatbot').hide(); // Keep chatbot hidden if it was closed
        $('#openButton').show(); // Show the open button
    }

    // Bind sendMessage function to button click
    $('#sendButton').on('click', sendMessage);
});

// Function to start the chat
function startChat() {
    const chatlog = $('#chatlog');
    chatlog.append(`<div class="botMessage"><strong>Bot:</strong> Vitajte! Ako vám môžem pomôcť dnes?</div>`);
    chatlog.scrollTop(chatlog.prop("scrollHeight"));
}

// Function to send a message
function sendMessage() {
    const userInput = $('#userInput').val();
    if (userInput.trim() === '') return; // Prevent sending empty messages

    // Display user message
    const chatlog = $('#chatlog');
    chatlog.append(`<div class="userMessage"><strong>You:</strong> ${userInput}</div>`);

    // Get bot response and display it
    const botResponse = getBotResponse(userInput);
    chatlog.append(`<div class="botMessage"><strong>Bot:</strong> ${botResponse}</div>`);

    // Clear the input field
    $('#userInput').val('');
    chatlog.scrollTop(chatlog.prop("scrollHeight")); // Scroll to the bottom
}

// Function to normalize string by removing diacritics
function removeDiacritics(str) {
    return str.normalize("NFD").replace(/[\u0300-\u036f]/g, "");
}

// Get the bot's response based on the input
function getBotResponse(input) {
    const normalizedInput = removeDiacritics(input.toLowerCase().trim());
    const responses = {
        "Ahoj": "Ahoj! Ako ti môžem pomôcť?",
        "Ako sa mas?": "Mám sa dobre, ďakujem! A ty?",
        "Co obsahuje tvoje portfolio?": "Moje portfólio obsahuje projekty, na ktorých som pracoval, a informácie o mojich zručnostiach.",
        "Ako mozem kontaktovat?": "Môžete ma kontaktovať cez kontaktný formulár na mojej stránke.",
        "Zivotopis?": "Áno, môj životopis je dostupný na stránke.",
        "Projekty?": "Realizoval som rôzne projekty od webových aplikácií po mobilné aplikácie.",
        "Referencie?": "Áno, referencie sú uvedené na stránke.",
        "Ako dlho IT?": "V oblasti IT sa pohybujem už viac ako 5 rokov.",
        "Ake technologie?": "Ovladám HTML, CSS, JavaScript, PHP a SQL.",
        "Skolenia?": "Áno, ponúkam školenia a konzultácie na rôzne témy.",
        "Silne stranky?": "Mojimi silnými stránkami sú technické zručnosti a kreatívny prístup.",
        "Custom projekt?": "Áno, rád vytvorím prispôsobený projekt podľa vašich potrieb.",
        "Oblubene projekty?": "Obľubujem projekty, kde môžem byť kreatívny a uplatniť svoje zručnosti.",
        "Socialne siete?": "Áno, odkazy na moje profily sú na stránke.",
        "Graficky dizajn?": "Áno, mám skúsenosti s grafickým dizajnom a brandovaním.",
        "Odpoved na spravu?": "Odpovedám na správy do 24 hodín.",
        "Navrh projektu?": "Áno, rád si prečítam vaše návrhy.",
        "Plany do buducnosti?": "Plánujem rozšíriť svoje portfólio a zručnosti.",
        "Kvalita projektov?": "Kvalitu zabezpečujem dôkladným testovaním pred spustením.",
        "UX/UI dizajn?": "UX/UI dizajn sa zameriava na používateľskú skúsenosť a rozhranie.",
        "Ukazky projektov?": "Áno, ukážky sú k dispozícii v sekcii 'Portfólio'.",
        "Najnovsie projekty?": "Najnovšie projekty sú uvedené v sekcii 'Novinky'.",
        "Platba?": "Platba je možná cez bankový prevod alebo online platby.",
        "Databazy?": "Áno, pracujem s rôznymi databázovými systémami.",
        "Oblubene technologie?": "Obľubujem moderné technológie ako React a Node.js.",
        "Konzultacia?": "Áno, rezervácie sú k dispozícii na stránke.",
        "Ocenenia?": "Áno, mám niekoľko certifikátov v oblasti webového vývoja.",
        "Uspechy?": "Môj najväčší úspech bol úspešný projekt pre významného klienta.",
        "Ceny?": "Áno, rád vám poskytnem informácie o cenách na požiadanie.",
        "Inspiracia?": "Inšpiruje ma technológia a inovatívne prístupy k dizajnu.",
        "Nastroje?": "Obľubujem Figma, Adobe XD a Visual Studio Code.",
        "Pomoc s webom?": "Áno, rád vám pomôžem s vašimi webovými projektmi.",
        "E-commerce?": "Áno, mám skúsenosti s vytváraním online obchodov.",
        "Doba dokoncienia?": "Doba dokončenia závisí od zložitosti projektu.",
        "Hodnoty?": "Mojimi hodnotami sú kvalita, kreativita a dôveryhodnosť.",
        "Ochrana sukromia?": "Zabezpečujem ochranu súkromia podľa platných zákonov.",
        "Navrh?": "Áno, rád si prečítam vaše návrhy.",
        "Brandovanie?": "Brandovanie je proces vytvárania identity značky.",
        "Problem s webom?": "Môžete mi povedať, aký je problém?",
        "Styl dizajnu?": "Môj štýl je moderný a užívateľsky prívetivý.",
        "Spolupraca?": "Môj proces je otvorený a komunikatívny.",
        "Dizajn loga?": "Áno, ponúkam aj dizajn loga.",
        "Ciel pri praci?": "Mojím cieľom je dosiahnuť spokojnosť zákazníka.",
        "Navrh webu?": "Áno, rád vám pomôžem s návrhom.",
        "Zaujem o sluzby?": "Skvelé! Kontaktujte ma cez stránku 'Kontakt'.",
        "Viac informacii?": "Ďalšie informácie sú k dispozícii na stránke.",
        "Vytvorenie webu?": "Áno, ponúkam služby webového dizajnu.",
        "Otazky?": "Áno, neváhajte sa ma opýtať na čokoľvek.",
        "Rezervacia?": "Áno, rezervácie sú k dispozícii na stránke.",
        "Starostlivost o klientov?": "Klientom sa snažím vychádzať v ústrety a poskytovať im podporu.",
        "Pristup k sablonam?": "Áno, šablóny sú k dispozícii na stiahnutie.",
        "Najlepsie projekty?": "Najlepšie projekty sú uvedené v sekcii 'Referencie'.",
        "Oblubene weby?": "Obľubujem moderné a prehľadné weby.",
        "Plany do buducnosti?": "Plánujem rozšíriť svoje služby a portfólio.",
        "Ceny?": "Rád vám poskytnem informácie o cenách.",
        "Navrh?": "Áno, rád si prečítam vaše návrhy.",
        "Dizajn webu?": "Áno, ponúkam služby webového dizajnu.",
        "Oblubeny projekt?": "Môj obľúbený projekt je web pre miestnu neziskovku.",
        "Trendy v dizajne?": "Mám rád minimalistický a funkčný dizajn.",
        "Pred zaciatim projektu?": "Je dobré mať predstavu o svojich požiadavkách a cieľoch.",
        "Kontakt cez e-mail?": "Áno, kontaktné údaje sú na stránke Kontakt.",
        "Zaujem o spolupracu?": "Skvelé! Kontaktujte ma cez stránku 'Kontakt'.",
        "Pomoc s navrhom?": "Áno, rád vám pomôžem s návrhom.",
        "Pristup k portfoliu?": "Moje portfólio nájdete na mojej webovej stránke.",
        "Dizajn sablon?": "Áno, dizajn šablón je súčasťou mojich služieb.",
        "Ake je tvoje motto?": "Motto je: 'Kvalita a kreativita na prvom mieste.'",
        "Ake su tvoje hodnoty?": "Mojimi hodnotami sú transparentnosť a dôveryhodnosť.",
        "Aky je tvoj pristup k praci?": "Mám systematický a efektívny prístup.",
        "Mozem si pozriet tvoje projekty?": "Áno, moje projekty sú v sekcii 'Portfólio'.",
        "Mozem ti poslat spravu?": "Áno, kontaktujte ma cez formulár.",
        "Kde sa nachadzas?": "Pracujem online, môžeme sa spojiť kedykoľvek.",
        "Mas nejake otazky?": "Samozrejme, aké otázky máš?",
        "Mozem sa teba na nieco opytat?": "Áno, neváhajte sa pýtať.",
        "Ako ta mozem oslovit?": "Môžete ma osloviť priamo menom alebo 'Chatbot'.",
        "Ako dlho trva projekt?": "Doba trvania závisí od jeho zložitosti.",
        "Mas nejake obmedzenia?": "Snažím sa vychádzať v ústrety všetkým požiadavkám.",
        "Kolko projektov si realizoval?": "Realizoval som množstvo projektov vo viacerých oblastiach.",
        "Ako sa máš?": "Mám sa dobre, ďakujem! A ty?",
        "Čo obsahuje tvoje portfólio?": "Moje portfólio obsahuje projekty, na ktorých som pracoval, a informácie o mojich zručnostiach.",
        "Ako môžem kontaktovať?": "Môžete ma kontaktovať cez kontaktný formulár na mojej stránke.",
        "Životopis?": "Áno, môj životopis je dostupný na stránke.",
        "Projekty?": "Realizoval som rôzne projekty od webových aplikácií po mobilné aplikácie.",
        "Referencie?": "Áno, referencie sú uvedené na stránke.",
        "Ako dlho IT?": "V oblasti IT sa pohybujem už viac ako 5 rokov.",
        "Aké technológie?": "Ovladám HTML, CSS, JavaScript, PHP a SQL.",
        "Školenia?": "Áno, ponúkam školenia a konzultácie na rôzne témy.",
        "Silné stránky?": "Mojimi silnými stránkami sú technické zručnosti a kreatívny prístup.",
        "Custom projekt?": "Áno, rád vytvorím prispôsobený projekt podľa vašich potrieb.",
        "Obľúbené projekty?": "Obľubujem projekty, kde môžem byť kreatívny a uplatniť svoje zručnosti.",
        "Sociálne siete?": "Áno, odkazy na moje profily sú na stránke.",
        "Grafický dizajn?": "Áno, mám skúsenosti s grafickým dizajnom a brandovaním.",
        "Odpoveď na správu?": "Odpovedám na správy do 24 hodín.",
        "Návrh projektu?": "Áno, rád si prečítam vaše návrhy.",
        "Plány do budúcnosti?": "Plánujem rozšíriť svoje portfólio a zručnosti.",
        "Kvalita projektov?": "Kvalitu zabezpečujem dôkladným testovaním pred spustením.",
        "UX/UI dizajn?": "UX/UI dizajn sa zameriava na používateľskú skúsenosť a rozhranie.",
        "Ukážky projektov?": "Áno, ukážky sú k dispozícii v sekcii 'Portfólio'.",
        "Najnovšie projekty?": "Najnovšie projekty sú uvedené v sekcii 'Novinky'.",
        "Platba?": "Platba je možná cez bankový prevod alebo online platby.",
        "Databázy?": "Áno, pracujem s rôznymi databázovými systémami.",
        "Obľúbené technológie?": "Obľubujem moderné technológie ako React a Node.js.",
        "Konzultácia?": "Áno, rezervácie sú k dispozícii na stránke.",
        "Ocenenia?": "Áno, mám niekoľko certifikátov v oblasti webového vývoja.",
        "Úspechy?": "Môj najväčší úspech bol úspešný projekt pre významného klienta.",
        "Ceny?": "Áno, rád vám poskytnem informácie o cenách na požiadanie.",
        "Inšpirácia?": "Inšpiruje ma technológia a inovatívne prístupy k dizajnu.",
        "Nástroje?": "Obľubujem Figma, Adobe XD a Visual Studio Code.",
        "Pomoc s webom?": "Áno, rád vám pomôžem s vašimi webovými projektmi.",
        "E-commerce?": "Áno, mám skúsenosti s vytváraním online obchodov.",
        "Doba dokončenia?": "Doba dokončenia závisí od zložitosti projektu.",
        "Hodnoty?": "Mojimi hodnotami sú kvalita, kreativita a dôveryhodnosť.",
        "Ochrana súkromia?": "Zabezpečujem ochranu súkromia podľa platných zákonov.",
        "Návrh?": "Áno, rád si prečítam vaše návrhy.",
        "Brandovanie?": "Brandovanie je proces vytvárania identity značky.",
        "Problém s webom?": "Môžete mi povedať, aký je problém?",
        "Štýl dizajnu?": "Môj štýl je moderný a užívateľsky prívetivý.",
        "Spolupráca?": "Môj proces je otvorený a komunikatívny.",
        "Dizajn loga?": "Áno, ponúkam aj dizajn loga.",
        "Cieľ pri práci?": "Mojím cieľom je dosiahnuť spokojnosť zákazníka.",
        "Návrh webu?": "Áno, rád vám pomôžem s návrhom.",
        "Záujem o služby?": "Skvelé! Kontaktujte ma cez stránku 'Kontakt'.",
        "Viac informácií?": "Ďalšie informácie sú k dispozícii na stránke.",
        "Vytvorenie webu?": "Áno, ponúkam služby webového dizajnu.",
        "Otázky?": "Áno, neváhajte sa ma opýtať na čokoľvek.",
        "Rezervácia?": "Áno, rezervácie sú k dispozícii na stránke.",
        "Starostlivosť o klientov?": "Klientom sa snažím vychádzať v ústrety a poskytovať im podporu.",
        "Prístup k šablónam?": "Áno, šablóny sú k dispozícii na stiahnutie.",
        "Najlepšie projekty?": "Najlepšie projekty sú uvedené v sekcii 'Referencie'.",
        "Obľúbené weby?": "Obľubujem moderné a prehľadné weby.",
        "Plány do budúcnosti?": "Plánujem rozšíriť svoje služby a portfólio.",
        "Ceny?": "Rád vám poskytnem informácie o cenách.",
        "Návrh?": "Áno, rád si prečítam vaše návrhy.",
        "co": "Ahoj! Ako ti môžem pomôcť?",
        "ako": "Ahoj! Ako ti môžem pomôcť?",
        "Co": "Ahoj! Ako ti môžem pomôcť?",
        "Ako": "Ahoj! Ako ti môžem pomôcť?", "co": "Ahoj! Ako ti môžem pomôcť?",
        "ako": "Ahoj! Ako ti môžem pomôcť?",
        "Pre": "Ahoj! Ako ti môžem pomôcť?",
        "Prečo": "Ahoj! Ako ti môžem pomôcť?",
        "Dizajn webu?": "Áno, ponúkam služby webového dizajnu.",
        "Obľúbený projekt?": "Môj obľúbený projekt je web pre miestnu neziskovku.",
        "Trendy v dizajne?": "Mám rád minimalistický a funkčný dizajn.",
        "Pred začiatím projektu?": "Je dobré mať predstavu o svojich požiadavkách a cieľoch.",
        "Kontakt cez e-mail?": "Áno, kontaktné údaje sú na stránke.",
        "Záujem o spoluprácu?": "Skvelé! Kontaktujte ma cez stránku 'Kontakt'.",
        "Pomoc s návrhom?": "Áno, rád vám pomôžem s návrhom.",
        "Prístup k portfóliu?": "Moje portfólio nájdete na mojej webovej stránke.",
        "Dizajn šablón?": "Áno, dizajn šablón je súčasťou mojich služieb.",
        "Aké je tvoje motto?": "Motto je: 'Kvalita a kreativita na prvom mieste.'",
        "Aké sú tvoje hodnoty?": "Mojimi hodnotami sú transparentnosť a dôveryhodnosť.",
        "Aký je tvoj prístup k práci?": "Mám systematický a efektívny prístup.",
        "Môžem si pozrieť tvoje projekty?": "Áno, moje projekty sú v sekcii 'Portfólio'.",
        "Môžem ti poslať správu?": "Áno, kontaktujte ma cez formulár.",
        "Kde sa nachádzaš?": "Pracujem online, môžeme sa spojiť kedykoľvek.",
        "Máš nejaké otázky?": "Samozrejme, aké otázky máš?",
        "Môžem sa teba na niečo opýtať?": "Áno, neváhajte sa pýtať.",
        "Ako ťa môžem osloviť?": "Môžete ma osloviť priamo menom alebo 'Chatbot'.",
        "Ako dlho trvá projekt?": "Doba trvania závisí od jeho zložitosti.",
        "Máš nejaké obmedzenia?": "Snažím sa vychádzať v ústrety všetkým požiadavkám.",
        "Koľko projektov si realizoval?": "Realizoval som množstvo projektov vo viacerých oblastiach.",
        "co je clenska sekcia?": "Clenska sekcia je miesto, kde najdete exkluzivny obsah a vyhody pre nasich clenov.",
        "ako sa mozem stat clenom?": "Mozete sa stat clenom vyplnenim registračneho formulara na nasej stránke.",
        "ake su vyhody clenstva?": "Clenstvo vam poskytuje pristup k exkluzivnym materialom, zlavam a roznym akciam.",
        "mozem zrusit clenstvo?": "Ano, clenstvo mozete zrusit kedykoľvek cez svoj profil v clenskej sekcii.",
        "co robit ak zabudnem heslo?": "Ak ste zabudli heslo, mozete ho obnovit cez odkaz na obnovenie hesla na prihlasovacej stránke.",
        "ako mozem aktualizovat svoj profil?": "Svoj profil mozete aktualizovat prihlasenim sa do clenskej sekcie a kliknutim na 'Upravit profil'.",
        "mozem sa teba na nieco opytat?": "Ano, nevahajte sa pytat.",
        "ako ta mozem oslovit?": "Mozete ma oslovit priamo menom alebo 'Chatbot'.",
        "ako dlho trva projekt?": "Doba trvania zavisi od jeho zlozitosti.",
        "mas nejake obmedzenia?": "Snažím sa vychadzat v ústrety vsetkym poziadavkam.",
        "kolko projektov si realizoval?": "Realizoval som mnozstvo projektov vo viacerych oblastiach.",
        "Co je clenska sekcia?": "Clenska sekcia je miesto, kde najdete exkluzivny obsah a vyhody pre nasich clenov.",
        "Ako sa mozem stat clenom?": "Mozete sa stat clenom vyplnenim registračneho formulara na nasej stránke.",
        "Ake su vyhody clenstva?": "Clenstvo vam poskytuje pristup k exkluzivnym materialom, zlavam a roznym akciam.",
        "Mozem zrusit clenstvo?": "Ano, clenstvo mozete zrusit kedykoľvek cez svoj profil v clenskej sekcii.",
        "Co robit ak zabudnem heslo?": "Ak ste zabudli heslo, mozete ho obnovit cez odkaz na obnovenie hesla na prihlasovacej stránke.",
        "Ako mozem aktualizovat svoj profil?": "Svoj profil mozete aktualizovat prihlasenim sa do clenskej sekcie a kliknutim na 'Upravit profil'.",
        "Mozem sa teba na nieco opytat?": "Ano, nevahajte sa pytat.",
        "Ako ta mozem oslovit?": "Mozete ma oslovit priamo menom alebo 'Chatbot'.",
        "Ako dlho trva projekt?": "Doba trvania zavisi od jeho zlozitosti.",
        "Mas nejake obmedzenia?": "Snažím sa vychadzat v ústrety vsetkym poziadavkam.",
        "Kolko projektov si realizoval?": "Realizoval som mnozstvo projektov vo viacerych oblastiach.",
    };

    const foundResponse = Object.keys(responses).find(key => normalizedInput.includes(removeDiacritics(key.toLowerCase()))); // Match case-insensitively
    return foundResponse ? responses[foundResponse] : "Prepáčte, nerozumiem. Môžete skúsiť inú otázku alebo sa opýtať konkrétnejšie.";
}
