using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public struct ClueEntry
{
    public string text, summary, nextClueID;

    public ClueEntry(string text, string summary, string next)
    {
        this.text = text;
        this.summary = summary;
        nextClueID = next;
    }
}

public class NPCDialogue : MonoBehaviour, IInteractable {

    public CharacterID id;
    Dictionary<string, ClueEntry> clueResponses;
    public string dialogueFile;
    bool firstMeeting = true;
    bool randomNPC;
    public bool likesBlack;
    string blackOpinion;
    public bool likesBlue;
    string blueOpinion;
    public bool likesGreen;
    string greenOpinion;
    public bool likesRed;
    string redOpinion;
    public bool likesYellow;
    string yellowOpinion;

    public bool hasHint;
    public CharacterID hintBlames;

    Quotes quotes;

	// Use this for initialization
	void Start () {
        quotes = Quotes.LoadJSON(dialogueFile);
        if (quotes != null)
        {
            if (quotes.introductions.Count == 0)
            {
                firstMeeting = false;
            }
        }
        if (id == CharacterID.RANDO || id == CharacterID.VICTIM)
        {
            randomNPC = true;
        }

        

        if (Characters.IsSuspect(id))
        {
            //clueResponses.Add("", new ClueEntry("", "", ""));
            clueResponses = new Dictionary<string, ClueEntry>();
            switch (id)
            {
                case CharacterID.BLACK:
                    //Motive
                    clueResponses.Add("motive", new ClueEntry("[Victim] was... less than religious.  I believe he was a fool to reject the Lord. | Look where that landed him.", "[Black] said: [Black] believes that [Victim] was a fool to reject religion.", "blackReligionBlack"));

                    clueResponses.Add("BLACKMOTIVEGREEN", new ClueEntry("Would you truly expect a critic of alcohol and a vintner to get along?", "[Black] said: [Green] and [Victim] had clashing opinion on alcohol.", "blackClashGreen"));
                    clueResponses.Add("BLACKMOTIVEYELLOW", new ClueEntry("Even those who do not follow the Lord must find solace in something. | Her comfort was found in [Victim] tending to her husband until the end.", "[Black] said: [Yellow] was happy that [Victim] made Mr. Yellow's last moments comfortable.", "blackHusbandYellow"));
                    clueResponses.Add("BLACKMOTIVERED", new ClueEntry("Their relationship appears to have been that of supply partners.  [Red]'s high quality art supplies often came from [Victim] importing them to [Red] when he visits. | In fact, much of the art around the church was made by [Red] using [Victim]'s supplies, so I doubt that their relationship was too sour.", "[Black] said: [Red] and [Victim] were supply partners.", "blackSupplierRed"));
                    clueResponses.Add("BLACKMOTIVEBLACK", new ClueEntry("[Victim] was... less than religious.  I believe he was a fool to reject the Lord. | Look where that landed him.", "[Black] said: [Black] believes that [Victim] was a fool to reject religion.", "blackReligionBlack"));
                    clueResponses.Add("BLACKMOTIVEBLUE", new ClueEntry("I cannot imagine that [Blue] cares for the man who promotes abstinence. | His confessionals, though, reveal a deeper hate of [Victim].  Why though, he would not say.", "[Black] said: [Blue] didn't like being judged by [Victim] for drinking.", "blackDrinkingBlue"));
                    //Motive Responses
                    clueResponses.Add("blueAngryBlack", new ClueEntry("[Victim] was... less than religious.  I believe he was a fool to reject the Lord. | Look where that landed him.", "[Black] said: [Black] believes that [Victim] was a fool to reject religion.", "blackReligionBlack"));
                    clueResponses.Add("greenHeadacheBlack", new ClueEntry("%(sigh)% Of course I was calmed when [Victim] visited.  I cannot hate a fellow saver of lives. | However, I stand by my word; he was a fool to hate the Lord.", "[Black] said: [Black] believes that [Victim] was a fool to reject religion.", "blackReligionBlack"));
                    clueResponses.Add("redLikedBlack", new ClueEntry("%(sigh)% Of course I was calmed when [Victim] visited.  I cannot hate a fellow saver of lives. | However, I stand by my word; he was a fool to hate the Lord.", "[Black] said: [Black] believes that [Victim] was a fool to reject religion.", "blackReligionBlack"));
                    clueResponses.Add("yellowHeadacheBlack", new ClueEntry("%(sigh)% Of course I was calmed when [Victim] visited.  I cannot hate a fellow saver of lives. | However, I stand by my word; he was a fool to hate the Lord.", "[Black] said: [Black] believes that [Victim] was a fool to reject religion.", "blackReligionBlack"));

                    //Opportunity
                    clueResponses.Add("opportunity", new ClueEntry("As a servent of the Lord, I must continually reconnect with Him. | I dedicated that night to this aim.", "[Black] said: [Black] was having a devotional in the Church that night.", "blackDevotionalBlack"));

                    clueResponses.Add("BLACKOPPORTUNITYGREEN", new ClueEntry("Before I went into the Lord's house for the night.  I saw [Green] at the Town Square. | He seems to defy the clock while working, doesn't he?", "[Black] said: He saw [Green] in the Town Square that night.", "blackSawGreen"));
                    clueResponses.Add("BLACKOPPORTUNITYYELLOW", new ClueEntry("In a break from my devotional, I saw her in the graveyard. | I know her husband is with the Lord, but I fear that she may not be joining him.  Perhaps she was coming to terms with that.", "[Black] said: He saw [Yellow] at the Graveyard that night.", "blackSawYellow"));
                    clueResponses.Add("BLACKOPPORTUNITYRED", new ClueEntry("I had hoped to speak with [Red] before I went into my devotion, but she was nowhere to be found. | She must have been out for the night, but I could not imagine where.", "[Black] said: He didn't see [Red] that night.", "blackDidntSeeRed"));
                    clueResponses.Add("BLACKOPPORTUNITYBLACK", new ClueEntry("As a servent of the Lord, I must continually reconnect with Him. | I dedicated that night to this aim.", "[Black] said: [Black] was having a devotional in the Church that night.", "blackDevotionalBlack"));
                    clueResponses.Add("BLACKOPPORTUNITYBLUE", new ClueEntry("[Blue]?  I failed to notice him on his usual rounds. | Perhaps he was slacking, as he is known to do.", "[Black] said: He didn't see [Blue] that night.", "blackDidntSeeBlue"));
                    //Opportunity Responses
                    clueResponses.Add("blueSawBlack", new ClueEntry("People are saying that I was out? | The night of [Victim]'s murder, I was having a devotional through the night at the center of the church. | I do not pretend to understand how anyone could misconstrue that.", "[Black] said: [Black] was having a devotional in the Church that night.", "blackDevotionalBlack"));
                    clueResponses.Add("greenSawBlack", new ClueEntry("The night of [Victim]'s murder, I was having a devotional through the night at the center of the church.", "[Black] said: [Black] was having a devotional in the Church that night.", "blackDevotionalBlack"));
                    clueResponses.Add("redDidntSeeBlack", new ClueEntry("People are saying that I was out? | The night of [Victim]'s murder, I was having a devotional through the night at the center of the church. | I do not pretend to understand how anyone could misconstrue that.", "[Black] said: [Black] was having a devotional in the Church that night.", "blackDevotionalBlack"));
                    clueResponses.Add("yellowSawBlack", new ClueEntry("The night of [Victim]'s murder, I was having a devotional through the night at the center of the church.", "[Black] said: [Black] was having a devotional in the Church that night.", "blackDevotionalBlack"));
                    //Weapon
                    clueResponses.Add("means", new ClueEntry("May the Lord have mercy on [Victim]'s soul. | ...You believe that the Lord's servant would have [Weapon]? | That is nothing more than false, child.  I'll hear no more of it. | My turn.  Did you know that [Yellow] has [Weapon]?  The Lord sees all.", "[Black] said: [Black] does not own [Weapon], but [Yellow] does.", "blackWeaponYellow"));

                    break;
                case CharacterID.BLUE:
                    //Motive
                    clueResponses.Add("motive", new ClueEntry("[Victim] was a playboy for the ages.  He couldn't keep his hands off of [Yellow]. | I'm not happy that he's dead though. %but at least she won't be harassed anymore.%", "[Blue] said: [Blue] didn't like that [Victim] harassed [Yellow].", "blueHarassYellow"));

                    clueResponses.Add("BLUEMOTIVEGREEN", new ClueEntry("[Green] liked wine, %who can blame him,% and [Victim] didn't.  What is there to figure out?", "[Blue] said: [Victim] was against what [Green] stood for.", "blueAgainstGreen"));
                    clueResponses.Add("BLUEMOTIVEYELLOW", new ClueEntry("I dunno.  Why are you askin' me about [Yellow]? %not like she talks to me anyway% | Well, she's been a bit colder to me since her husband died.  Maybe she blames [Victim] for his death?", "[Blue] said: [Yellow] blamed [Victim] for her husband's death.", "blueHusbandYellow"));
                    clueResponses.Add("BLUEMOTIVERED", new ClueEntry("%(sigh)% Geez, why should I know? | I guess I saw [Victim] purchasing supplies from [Red] each time he was in town.  %not like her keeper or anything%", "[Blue] said: [Victim] often purchased supplies from [Red].", "blueSupplierRed"));
                    clueResponses.Add("BLUEMOTIVEBLACK", new ClueEntry("I think [Victim] gave the old man some news he didn't quite like. | Ever since a particular meeting, [Black] seems too angry for words when [Victim] is brought up.", "[Blue] said: [Victim] really ticked [Black] off somehow.", "blueAngryBlack"));
                    clueResponses.Add("BLUEMOTIVEBLUE", new ClueEntry("[Victim] was a playboy for the ages.  He couldn't keep his hands off of [Yellow]. | I'm not happy that he's dead though. %but at least she won't be harassed anymore.%", "[Blue] said: [Blue] didn't like that [Victim] harassed [Yellow].", "blueHarassYellow"));
                    //Motive Responses
                    clueResponses.Add("blackDrinkingBlue", new ClueEntry("%here we go.% [Victim] was the only person who could rival [Black] in judginess. | What I do, what I drink, who I spend my time with, he always made a point to comment on them. %at least I get why the friar's doing it.% | Like he's one to talk, pointing his hips at [Yellow] any time she's in the room. | %(sigh)% I love how everything's calm enough to take a day off when he visits, and he seems like a good doctor, but he always got on my nerves when I actually needed a checkup.", "[Blue] said: [Blue] didn't like that [Victim] harassed [Yellow].", "blueHarassYellow"));
                    clueResponses.Add("greenSlackingBlue", new ClueEntry("%here we go.% [Victim] was the only person who could rival [Black] in judginess. | What I do, what I drink, who I spend my time with, he always made a point to comment on them. %at least I get why the friar's doing it.% | Like he's one to talk, pointing his hips at [Yellow] any time she's in the room. | %(sigh)% I love how everything's calm enough to take a day off when he visits, and he seems like a good doctor, but he always got on my nerves when I actually needed a checkup.", "[Blue] said: [Blue] didn't like that [Victim] harassed [Yellow].", "blueHarassYellow"));
                    clueResponses.Add("redJealousBlue", new ClueEntry("[Victim] was a playboy for the ages.  He couldn't keep his hands off of [Yellow]. | I'm not happy that he's dead though. %but at least she won't be harassed anymore.%", "[Blue] said: [Blue] didn't like that [Victim] harassed [Yellow].", "blueHarassYellow"));
                    clueResponses.Add("yellowFaceBlue", new ClueEntry("[Victim] was a playboy for the ages.  He couldn't keep his hands off of [Yellow]. | I'm not happy that he's dead though. %but at least she won't be harassed anymore.%", "[Blue] said: [Blue] didn't like that [Victim] harassed [Yellow].", "blueHarassYellow"));

                    //Opportunity
                    clueResponses.Add("opportunity", new ClueEntry("I was.... I was passed out that night.  The bottle hit me harder than usual.", "[Blue] said: [Blue] was passed drunk out that night.", "blueDrunkBlue"));

                    clueResponses.Add("BLUEOPPORTUNITYGREEN", new ClueEntry("%hiccup% Who can keep track with all that he does? | %hiccup% But, most assuredly he was %hiccup% near the bakery.", "[Blue] said: He saw [Green] near Yellow Bakery.", "blueSawGreen"));
                    clueResponses.Add("BLUEOPPORTUNITYYELLOW", new ClueEntry("I didn't spot her that night.  And believe me, I tried. | ([Blue] shoots you a strange wink)", "[Blue] said: He didn't see [Yellow] that night.", "blueDidntSeeYellow"));
                    clueResponses.Add("BLUEOPPORTUNITYRED", new ClueEntry("Where was [Red], you ask? | ...Hm... %she didn't bug me that evening like usual.% | I, uh, don't know where she went.", "[Blue] said: He didn't see [Red] that night.", "blueDidntSeeRed"));
                    clueResponses.Add("BLUEOPPORTUNITYBLACK", new ClueEntry("[Black]?  He was out that night, sure. | Why?  Was he somewhere else?", "[Blue] said: He saw [Black] out and around that night.", "blueSawBlack"));
                    clueResponses.Add("BLUEOPPORTUNITYBLUE", new ClueEntry("I was.... I was passed out that night.  The bottle hit me harder than usual.", "[Blue] said: [Blue] was passed drunk out that night.", "blueDrunkBlue"));
                    //Opportunity Responses
                    clueResponses.Add("blackDidntSeeBlue", new ClueEntry("Huh? %didn't I tell you...?% | I was passed out that night from living it up with some bottles of Green Wine. | I woke up this morning on a bench near the West entrance of town, went home to clean up, and then went back out to work. | I don't know about you, but do my habbits really seem murderous?", "[Blue] said: [Blue] was passed drunk out that night.", "blueDrunkBlue"));
                    clueResponses.Add("greenDidntSeeBlue", new ClueEntry("Huh? %didn't I tell you...?% | I was passed out that night from living it up with some bottles of Green Wine. | I woke up this morning on a bench near the West entrance of town, went home to clean up, and then went back out to work. | I don't know about you, but do my habbits really seem murderous?", "[Blue] said: [Blue] was passed drunk out that night.", "blueDrunkBlue"));
                    clueResponses.Add("redDidntSeeBlue", new ClueEntry("Huh? %didn't I tell you...?% | I was passed out that night from living it up with some bottles of Green Wine. | I woke up this morning on a bench near the West entrance of town, went home to clean up, and then went back out to work. | I don't know about you, but do my habbits really seem murderous?", "[Blue] said: [Blue] was passed drunk out that night.", "blueDrunkBlue"));
                    clueResponses.Add("yellowDidntSeeBlue", new ClueEntry("Huh? %didn't I tell you...?% | I was passed out that night from living it up with some bottles of Green Wine. | I woke up this morning on a bench near the West entrance of town, went home to clean up, and then went back out to work. | I don't know about you, but do my habbits really seem murderous?", "[Blue] said: [Blue] was passed drunk out that night.", "blueDrunkBlue"));

                    //Weapon
                    clueResponses.Add("means", new ClueEntry("%geez%  I dunno about that, detective. | I bet you want to know if I have something that could've strangled [Victim], huh babe? | Well don't worry, I wouldn't touch the thing.  Unless you wanted me to, of course. | I actually caught a glimpse of [Yellow] with [Weapon] yesterday.  %but you already knew that, didn't you?% | %smart bastard.%", "[Blue] said: [Blue] does not own [Weapon], but [Yellow] does.", "blueWeaponYellow"));
                    //Weapon Responses
                    clueResponses.Add("greenWeaponBlue", new ClueEntry("Don't worry, I wouldn't touch the thing.  Unless you wanted me to, of course. | I actually caught a glimpse of [Yellow] with [Weapon] yesterday.  %but you already knew that, didn't you?% | %smart bastard.%", "[Blue] said: [Blue] does not own [Weapon], but [Yellow] does.", "blueWeaponYellow"));
                    clueResponses.Add("redWeaponBlue", new ClueEntry("Don't worry, I wouldn't touch the thing.  Unless you wanted me to, of course. | I actually caught a glimpse of [Yellow] with [Weapon] yesterday.  %but you already knew that, didn't you?% | %smart bastard.%", "[Blue] said: [Blue] does not own [Weapon], but [Yellow] does.", "blueWeaponYellow"));

                    break;
                case CharacterID.GREEN:
                    //Motive
                    clueResponses.Add("motive", new ClueEntry("[Victim] may have been my personal doctor, but I assure you that it was purely ... out of necessity.", "[Green] said: [Victim] was [Green]'s personal doctor.", "greenDoctorGreen"));

                    clueResponses.Add("GREENMOTIVEGREEN", new ClueEntry("[Victim] may have been my personal doctor, but I assure you that it was purely ... out of necessity.", "[Green] said: [Victim] was [Green]'s personal doctor.", "greenDoctorGreen"));
                    clueResponses.Add("GREENMOTIVEYELLOW", new ClueEntry("It's a bit ... tragic really.  [Yellow]'s husband died under [Victim]'s care. | I doubt she ever really got over that...", "[Green] said: [Yellow]'s husband died under [Victim]'s care.", "greenHusbandYellow"));
                    clueResponses.Add("GREENMOTIVERED", new ClueEntry("They were business partners. | [Victim] brought art supplies and medical skills, [Red] supplied finished art, chocolates, and crafts. | The partnership appears mutual.  I doubt that [Red] would kill her ... supplier.", "[Green] said: [Victim] was [Red]'s craft supplier.", "greenSupplierRed"));
                    clueResponses.Add("GREENMOTIVEBLACK", new ClueEntry("When I brought [Victim] in, [Black] seemed to be more ... relaxed. | I suppose it saved him the headache of having to deal with the deceased.", "[Green] says: [Black] was calmed by [Victim]'s visits.", "greenHeadacheBlack"));
                    clueResponses.Add("GREENMOTVIEBLUE", new ClueEntry("A visit from [Victim] seems to calm everyone down.  Naturally, [Blue] seems laziest during [Victim]'s visits. | I doubt that he loathes the ... days off.", "[Blue] enjoys slacking when [Victim] visits.", "greenSlackBlue"));
                    //Motive Responses
                    clueResponses.Add("redExpeditedGreen", new ClueEntry("[Victim] was my personal doctor, and I assure you that it was purely ... out of necessity.", "[Green] said: [Victim] was [Green]'s personal doctor.", "greenDoctorGreen"));
                    clueResponses.Add("yellowExpeditedGreen", new ClueEntry("[Victim] was my personal doctor, and I assure you that it was purely ... out of necessity.", "[Green] said: [Victim] was [Green]'s personal doctor.", "greenDoctorGreen"));
                    clueResponses.Add("blackClashGreen", new ClueEntry("Ah yes.  [Victim]'s \"shows\". | [Victim] is a travelling doctor, but he also had theories that he loved to share.  So he would often claim a stage at a local tavern and speak of his discoveries. | His most recent and most famous \"discovery\" is that wine and alcohol have long-term negative health effects.  He essentially launched a war on alcohol. | However, [Victim] has long been my doctor, and he is luckily professional enough to not speak of it while he visits me, but these shows have cost Green Wine much in the past few years.", "[Green] said: [Victim] was [Green]'s personal doctor.", "greenDoctorGreen"));
                    clueResponses.Add("blueAgainstGreen", new ClueEntry("Ah yes.  [Victim]'s \"shows\". | [Victim] is a travelling doctor, but he also had theories that he loved to share.  So he would often claim a stage at a local tavern and speak of his discoveries. | His most recent and most famous \"discovery\" is that wine and alcohol have long-term negative health effects.  He essentially launched a war on alcohol. | However, [Victim] has long been my doctor, and he is luckily professional enough to not speak of it while he visits me, but these shows have cost Green Wine much in the past few years.", "[Green] said: [Victim] was [Green]'s personal doctor.", "greenDoctorGreen"));

                    //Opportunity
                    clueResponses.Add("opportunity", new ClueEntry("Actually, I was heading to [Yellow]'s Bakery on the night of the murder. | I'd heard that she wasn't going to the party that night, so I was going to ... meet with her about wine shipments, but she wasn't there so I walked straight home.", "[Green] said: [Green] went to Yellow Bakery and then walked straight home that night.", "greenSawYellow"));

                    clueResponses.Add("GREENOPPORTUNITYGREEN", new ClueEntry("Actually, I was heading to [Yellow]'s Bakery on the night of the murder. | I'd heard that she wasn't going to the party that night, so I was going to ... meet with her about wine shipments, but she wasn't there so I walked straight home.", "[Green] said: [Green] went to Yellow Bakery and then walked straight home that night.", "greenSawYellow"));
                    clueResponses.Add("GREENOPPORTUNITYYELLOW", new ClueEntry("I didn't see her at the bakery, but when I was heading back I saw her at the graveyard. | I figured we could ... reschedule.", "[Green] said: He saw [Yellow] visiting a grave in the Graveyard that night.", "greenSawYellow"));
                    clueResponses.Add("GREENOPPORTUNITYRED", new ClueEntry("On my way to the bakery I believe I saw [Red] in an alley.  She appeared to be making her exquisite artwork.", "[Green] said: He saw [Red] painting in an Alley that night.", "greenSawRed"));
                    clueResponses.Add("GREENOPPORTUNITYBLACK", new ClueEntry("I heard the friar say that he was going to have a ... devotional that night. | I had dropped by his church before the party started and saw him in deep prayer. | I have no reason to have expected him to move from that spot for the rest of the night.", "[Green] said: He saw [Black] having a devotional in the Church that night.", "greenSawBlack"));
                    clueResponses.Add("GREENOPPORTUNITYBLUE", new ClueEntry("I cannot recall seeing him in his usual spot, or at all that night, really.", "[Green] said: He didn't see [Blue] that night.", "greenDidntSeeBlue"));
                    //Opportunity Responses
                    clueResponses.Add("blackDidntSeeGreen", new ClueEntry("What accusation is this?  Me, at the scene of the crime? | True, I was out that night, but I took the Southern path from the Green Wine Winery to Yellow Bakery. | I was never in the Town Square that night, and I have a feeling that your accuser knows this.", "[Green] said: [Green] went to Yellow Bakery and then walked straight home that night.", "greenSawYellow"));
                    clueResponses.Add("blueSawGreen", new ClueEntry("I was heading to [Yellow]'s Bakery on the night of the murder. | I'd heard that she wasn't going to the party that night, so I was going to ... meet with her about wine shipments, but she wasn't there so I walked straight home.", "[Green] said: [Green] went to Yellow Bakery and then walked straight home that night.", "greenSawYellow"));
                    clueResponses.Add("redSawGreen", new ClueEntry("I was heading to [Yellow]'s Bakery on the night of the murder. | I'd heard that she wasn't going to the party that night, so I was going to ... meet with her about wine shipments, but she wasn't there so I walked straight home.", "[Green] said: [Green] went to Yellow Bakery and then walked straight home that night.", "greenSawYellow"));
                    clueResponses.Add("yellowDidntSeeGreen", new ClueEntry("Actually, I was heading to [Yellow]'s Bakery on the night of the murder. | I was going to ... meet with her about wine shipments, but she wasn't there so I walked straight home.", "[Green] said: [Green] went to Yellow Bakery and then walked straight home that night.", "greenSawYellow"));

                    //Weapon
                    clueResponses.Add("means", new ClueEntry("You wish to know if I own anything that could have ... strangled [Victim]? | Of course I ... have [Weapon], detective.  Don't you? | Since you seem to be ... having trouble, then I will give you the hint you so desire, detective.  [Blue] with [Weapon] at Town Square.", "[Green] said: [Green] owns [Weapon] and thinks [Blue] does too.", "greenWeaponBlue"));

                    break;
                case CharacterID.RED:
                    //Motive
                    clueResponses.Add("motive", new ClueEntry("I think [Victim] bought things from me just to turn around and charge me for his medical services. | [Victim] wasn't exactly the most curtious of shoppers though.  Always picking apart my worship of God.", "[Red] said: [Victim] was a bad customer to [Red].", "redMotiveRed"));

                    clueResponses.Add("REDMOTIVEGREEN", new ClueEntry("[Green] has the money to expedite [Victim]'s services when needed. | %must be nice having that kind of money.%", "[Red] said: [Green] expedited [Victim]'s visits when he needed him.", "redExpeditedGreen"));
                    clueResponses.Add("REDMOTIVEYELLOW", new ClueEntry("Can't blame her for hating [Victim]. | With the amount of money that [Victim] took as payment despite failing to keep [Yellow]'s husband alive, I'd hate the bastard too.", "[Red] said: [Yellow] was charged after [Victim] failed to save her husband.", "redHusbandYellow"));
                    clueResponses.Add("REDMOTIVERED", new ClueEntry("I think [Victim] bought things from me just to turn around and charge me for his medical services. | [Victim] wasn't exactly the most curtious of shoppers though.  Always picking apart my worship of God.", "[Red] said: [Victim] was a bad customer to [Red].", "redMotiveRed"));
                    clueResponses.Add("REDMOTIVEBLACK", new ClueEntry("The friar may not like most people, but he can't really complain about someone that's done so much good for the town.", "[Red] said: [Black] liked the good that [Victim] did for the town.", "redLikedBlack"));
                    clueResponses.Add("REDMOTIVEBLUE", new ClueEntry("Rumor has it that [Yellow] had a thing for [Victim], as messed up as that sounds. | Even worse, I think [Victim] returned the feelings. | So yeah, you guessed it, [Blue] hates him. %(sigh)%", "[Red] said: [Blue] is jealous of [Victim]'s love life.", "redJealousBlue"));
                    //Motive Responses
                    clueResponses.Add("blackSupplierRed", new ClueEntry("Yeah, yeah.  [Victim] was a supplier of mine, if you could call it that. | Just because he brought me stuff doesn't mean he had good intentions though. | Turns out I may have been the only one unable to pay his fees.  So in response, he started supplying me so that I could pay him on his next visits. | I don't mind the supplies, but [Victim]'s got problems.", "[Red] said: [Victim] was a bad customer to [Red].", "redMotiveRed"));
                    clueResponses.Add("greenSupplierRed", new ClueEntry("Yeah, yeah.  [Victim] was a supplier of mine, if you could call it that. | Just because he brought me stuff doesn't mean he had good intentions though. | Turns out I may have been the only one unable to pay his fees.  So in response, he started supplying me so that I could pay him on his next visits. | I don't mind the supplies, but [Victim]'s got problems.", "[Red] said: [Victim] was a bad customer to [Red].", "redMotiveRed"));
                    clueResponses.Add("yellowReligionRed", new ClueEntry("[Victim] wasn't exactly the most curtious of shoppers...  He was always picking apart my worship of God.", "[Red] said: [Victim] was a bad customer to [Red].", "redMotiveRed"));
                    clueResponses.Add("blueSupplierRed", new ClueEntry("Yeah, yeah.  [Victim] was a supplier of mine, if you could call it that. | Just because he brought me stuff doesn't mean he had good intentions though. | Turns out I may have been the only one unable to pay his fees.  So in response, he started supplying me so that I could pay him on his next visits. | I don't mind the supplies, but [Victim]'s got problems.", "[Red] said: [Victim] was a bad customer to [Red].", "redMotiveRed"));

                    //Opportunity
                    clueResponses.Add("opportunity", new ClueEntry("What?  You think I did it?  Of course I didn't do it! | If you must know, I was outside painting to keep from staining my floor.", "[Red] said: She was outside painting to avoid staining her floor.", "redOutsideRed"));

                    clueResponses.Add("REDOPPORTUNITYGREEN", new ClueEntry("I saw [Green] heading towards [Yellow]'s place that night.  If I weren't busy that night then I may have followed him. | Not for anything bad, of course.  I just don't trust those two together.", "[Red] said: She saw [Green] heading to Yellow Bakery that night.", "redSawGreen"));
                    clueResponses.Add("REDOPPORTUNITYYELLOW", new ClueEntry("I didn't see her leave the graveyard all night. | ...not that I was following her or anything.  I just happened to be nearby.", "[Red] said: She saw [Yellow] in the graveyard that night.", "redSawYellow"));
                    clueResponses.Add("REDOPPORTUNITYRED", new ClueEntry("What?  You think I did it?  Of course I didn't do it! | If you must know, I was outside painting to keep from staining my floor.", "[Red] said: She was outside painting to avoid staining her floor.", "redOutsideRed"));
                    clueResponses.Add("REDOPPORTUNITYBLACK", new ClueEntry("Usually he heads back into church after a long day, but I never saw him go in for the night.", "[Red] said: She didn't see [Black] in the Church that night.", "redDidntSeeBlack"));
                    clueResponses.Add("REDOPPORTUNITYBLUE", new ClueEntry("I didn't see him, though I'd bet he was getting drunk somewhere. | When's that idiot going to shape up?", "[Red] said: She didn't see [Blue] that night.", "redDidntSeeBlue"));
                    //Opportunity Responses
                    clueResponses.Add("blackDidntSeeRed", new ClueEntry("Where'd you get that accusation?  Give me a name! | Well, you know what?  I didn't murder [Victim], because I wasn't in the Town Square. | I got inspired and went out to paint by torchlight in an alley near my house. | I had a view of some entrances, but not the actual square and I have the art to prove it.  Tell my accuser that!", "[Red] said: She was outside painting to avoid staining her floor.", "redOutsideRed"));
                    clueResponses.Add("blueDidntSeeRed", new ClueEntry("Where'd you get that accusation?  Give me a name! | Well, you know what?  I didn't murder [Victim], because I wasn't in the Town Square. | I got inspired and went out to paint by torchlight in an alley near my house. | I had a view of some entrances, but not the actual square and I have the art to prove it.  Tell my accuser that!", "[Red] said: She was outside painting to avoid staining her floor.", "redOutsideRed"));
                    clueResponses.Add("greenSawRed", new ClueEntry("What?  You think I did it?  Of course I didn't do it! | I was outside painting to keep from staining my floor.", "[Red] said: She was outside painting to avoid staining her floor.", "redOutsideRed"));
                    clueResponses.Add("yellowSawRed", new ClueEntry("What?  You think I did it?  Of course I didn't do it! | I was outside painting to keep from staining my floor.", "[Red] said: She was outside painting to avoid staining her floor.", "redOutsideRed"));

                    //Weapon
                    clueResponses.Add("means", new ClueEntry("%(sigh)% What a mess. | ...Wait, you think that I have [Weapon]? | Yes, I have [Weapon].  Is that really your major breakthrough here? | I have a reason to have [Weapon] though, you should be asking [Blue] about [Weapon] instead. | You know, like a detective would.", "[Red] said: [Red] owns [Weapon] and thinks [Blue] does too.", "redWeaponBlue"));

                    break;
                case CharacterID.YELLOW:
                    //Motive
                    clueResponses.Add("motive", new ClueEntry("[Victim] seemed like a kind man.  However, %and pardon my bluntness, honey,% [Victim] was not a good doctor. | He is what we had though.  I doubt anyone is looking forward to the upcoming months without a doctor.", "[Yellow] said: [Yellow] thinks that [Victim] was a bad doctor.", "yellowHusbandYellow"));

                    clueResponses.Add("YELLOWMOTIVEGREEN", new ClueEntry("I don't know about their personal relationship, but I know that [Victim] only comes into town when [Green] expedites him. | [Green] must really prefer [Victim] to be his doctor.  Don't you think, honey?", "[Yellow] said: [Green] expedited [Victim]'s visits when he needed him.", "yellowExpeditedGreen"));
                    clueResponses.Add("YELLOWMOTIVEYELLOW", new ClueEntry("[Victim] seemed like a kind man.  However, %and pardon my bluntness, honey,% [Victim] was not a good doctor. | He is what we had though.  I doubt anyone is looking forward to the upcoming months without a doctor.", "[Yellow] said: [Yellow] thinks that [Victim] was a bad doctor.", "yellowHusbandYellow"));
                    clueResponses.Add("YELLOWMOTIVERED", new ClueEntry("I really shouldn't say anything, but... | Well, [Victim] was a very outspoken athiest, and you know [Red], hot tempered and religious. | I can't imagine their conversations lasted long or ended well.", "[Yellow] said: [Red] and [Victim]'s religious views clashed.", "yellowReligionRed"));
                    clueResponses.Add("YELLOWMOTIVEBLACK", new ClueEntry("Ah, yes, the friar didn't seem to have any major problems with [Victim]. | In fact, he seemed less sour than usual when [Victim] was in town.  Probably took some of the stress of death off his mind.", "[Yellow] said: [Black] seemed calmed by [Victim]'s visits.", "yellowHeadacheBlack"));
                    clueResponses.Add("YELLOWMOTIVEBLUE", new ClueEntry("Oh, that [Blue].  His face turns red as an apple pie when he sees [Victim] at my bakery. | [Victim] seems very respectful and is always such a sweetie to me.  I can't imagine why [Blue] wouldn't like him.", "[Yellow] said: [Blue] didn't like [Victim], for some reason.", "yellowFaceBlue"));
                    //Motive Responses
                    clueResponses.Add("greenHusbandYellow", new ClueEntry("[Victim] seemed like a kind man.  However, %and pardon my bluntness, honey,% [Victim] was not a good doctor. | He is what we had though.  I doubt anyone is looking forward to the upcoming months without a doctor.", "[Yellow] said: [Yellow] thinks that [Victim] was a bad doctor.", "yellowHusbandYellow"));
                    clueResponses.Add("redHusbandYellow", new ClueEntry("[Victim] seemed like a kind man.  However, %and pardon my bluntness, honey,% [Victim] was not a good doctor. | He is what we had though.  I doubt anyone is looking forward to the upcoming months without a doctor.", "[Yellow] said: [Yellow] thinks that [Victim] was a bad doctor.", "yellowHusbandYellow"));
                    clueResponses.Add("blackHusbandYellow", new ClueEntry("[Victim] seemed like a kind man.  However, %and pardon my bluntness, honey,% [Victim] was not a good doctor. | He is what we had though.  I doubt anyone is looking forward to the upcoming months without a doctor.", "[Yellow] said: [Yellow] thinks that [Victim] was a bad doctor.", "yellowHusbandYellow"));
                    clueResponses.Add("blueHusbandYellow", new ClueEntry("[Victim] seemed like a kind man.  However, %and pardon my bluntness, honey,% [Victim] was not a good doctor. | He is what we had though.  I doubt anyone is looking forward to the upcoming months without a doctor.", "[Yellow] said: [Yellow] thinks that [Victim] was a bad doctor.", "yellowHusbandYellow"));
                    //Opportunity
                    clueResponses.Add("opportunity", new ClueEntry("I went to graveyard that night, dearie.  I hadn't been to see Mr. [Yellow] in some time...", "[Yellow] said: [Yellow] was visiting her husband's grave at the Graveyard that night.", "yellowGraveyardYellow"));

                    clueResponses.Add("YELLOWOPPORTUNITYGREEN", new ClueEntry("[Green] was supposed to stop by to \"talk business, \" but he didn't show. | How strange though, he never misses an appointment.  Do you know what happend to him, dearie?", "[Yellow] said: She didn't see [Green] that night.", "yellowDidntSeeGreen"));
                    clueResponses.Add("YELLOWOPPORTUNITYYELLOW", new ClueEntry("I went to graveyard that night, dearie.  I hadn't been to see Mr. [Yellow] in some time...", "[Yellow] said: [Yellow] was visiting her husband's grave at the Graveyard that night.", "yellowGraveyardYellow"));
                    clueResponses.Add("YELLOWOPPORTUNITYRED", new ClueEntry("Oh, [Red]?  I believe I saw her painting while I was out. | She must have been really inspired to be working so late.", "[Yellow] said: She saw [Red] painting outside that night.", "yellowSawRed"));
                    clueResponses.Add("YELLOWOPPORTUNITYBLACK", new ClueEntry("You're asking about [Black], dearie?  I saw him in the church while I was out that night.  He seemed quite deep in prayer.", "[Yellow] said: She saw [Black] praying at the Church that night.", "yellowSawBlack"));
                    clueResponses.Add("YELLOWOPPORTUNITYBLUE", new ClueEntry("I did not see [Blue] that night, sorry honey. | He recently made a big purchase of wine so I'm sure he was just busy drinking.", "[Yellow] said: She didn't see [Blue] that night.", "yellowDidntSeeBlue"));
                    //Opportunity Responses
                    clueResponses.Add("blackSawYellow", new ClueEntry("Yes, it's true that I was at the graveyard that night.  I hadn't been to see Mr. [Yellow] in some time...", "[Yellow] said: [Yellow] was visiting her husband's grave at the Graveyard that night.", "yellowGraveyardYellow"));
                    clueResponses.Add("blueDidntSeeYellow", new ClueEntry("Oh dear! Someone thinks that I could have done it? What a terrible thing to say! | I closed up shop a bit late after finishing cooking for the party that night, and then walked directly to the Graveyard before it got late. | Time flew while I was there, so I was there most of the night. I went home the long way to the West of the Town Square late that night.", "[Yellow] said: [Yellow] was visiting her husband's grave at the Graveyard that night.", "yellowGraveyardYellow"));
                    clueResponses.Add("greenSawYellow", new ClueEntry("Yes, it's true that I was at the graveyard that night.  I hadn't been to see Mr. [Yellow] in some time...", "[Yellow] said: [Yellow] was visiting her husband's grave at the Graveyard that night.", "yellowGraveyardYellow"));
                    clueResponses.Add("redSawYellow", new ClueEntry("Yes, it's true that I was at the graveyard that night.  I hadn't been to see Mr. [Yellow] in some time...", "[Yellow] said: [Yellow] was visiting her husband's grave at the Graveyard that night.", "yellowGraveyardYellow"));
                    //Weapon
                    clueResponses.Add("means", new ClueEntry("Oh, how horrid!I can't believe that someone would do such a thing! | ...Oh, you want to know if I have something that could have killed [Victim]... | Mr. Yellow got [Weapon] awhile back.  I think I still have [Weapon] somewhere.", "[Yellow] said: [Yellow] owns [Weapon].", "yellowWeaponYellow"));
                    //Weapon Responses
                    clueResponses.Add("blueWeaponYellow", new ClueEntry("Mr. Yellow got [Weapon] awhile back.  I think I still have [Weapon] somewhere.", "[Yellow] said: [Yellow] owns [Weapon].", "yellowWeaponYellow"));
                    clueResponses.Add("blackWeaponYellow", new ClueEntry("Mr. Yellow got [Weapon] awhile back.  I think I still have [Weapon] somewhere.", "[Yellow] said: [Yellow] owns [Weapon].", "yellowWeaponYellow"));

                    break;
            }
            

            switch (id)
            {
                case CharacterID.BLACK:
                    likesBlack = true;
                    break;
                case CharacterID.BLUE:
                    likesBlue = true;
                    break;
                case CharacterID.GREEN:
                    likesGreen = true;
                    break;
                case CharacterID.RED:
                    likesRed = true;
                    break;
                case CharacterID.YELLOW:
                    likesYellow = true;
                    break;
            }
            if (likesBlack)
            {
                blackOpinion = quotes.motiveBlackInnocent[Random.Range(0, quotes.motiveBlackInnocent.Count)];
            }
            else
            {
                blackOpinion = quotes.motiveBlackGuilty[Random.Range(0, quotes.motiveBlackGuilty.Count)];
            }
            if (likesBlue)
            {
                blueOpinion = quotes.motiveBlueInnocent[Random.Range(0, quotes.motiveBlueInnocent.Count)];
            }
            else
            {
                blueOpinion = quotes.motiveBlueGuilty[Random.Range(0, quotes.motiveBlueGuilty.Count)];
            }
            if (likesGreen)
            {
                greenOpinion = quotes.motiveGreenInnocent[Random.Range(0, quotes.motiveGreenInnocent.Count)];
            }
            else
            {
                greenOpinion = quotes.motiveGreenGuilty[Random.Range(0, quotes.motiveGreenGuilty.Count)];
            }
            if (likesRed)
            {
                redOpinion = quotes.motiveRedInnocent[Random.Range(0, quotes.motiveRedInnocent.Count)];
            }
            else
            {
                redOpinion = quotes.motiveRedGuilty[Random.Range(0, quotes.motiveRedGuilty.Count)];
            }
            if (likesYellow)
            {
                yellowOpinion = quotes.motiveYellowInnocent[Random.Range(0, quotes.motiveYellowInnocent.Count)];
            }
            else
            {
                yellowOpinion = quotes.motiveYellowGuilty[Random.Range(0, quotes.motiveYellowGuilty.Count)];
            }
        }
        
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Interact()
    {
        if(quotes != null)
        {
            if(id == CharacterID.VICTIM)
            {
                StartCoroutine("ExamineBody");

            }
            DialogueSystem.Instance().OpenDialogueBox(id, this, firstMeeting, randomNPC);
            firstMeeting = false;
        }
        else
        {
            Provider.GetInstance().player.EndInteraction();
        }
    }

    public string GetIntroduction()
    {
        return quotes.introductions[Random.Range(0, quotes.introductions.Count)];
    }

    public string GetOpener()
    {
        return quotes.openers[Random.Range(0, quotes.openers.Count)];
    }

    public string GetCloser()
    {
        return quotes.closers[Random.Range(0, quotes.closers.Count)];
    }

    public string GetOpinion(CharacterID subject)
    {
        switch (subject)
        {
            case CharacterID.BLACK:
                return blackOpinion;
            case CharacterID.BLUE:
                return blueOpinion;
            case CharacterID.GREEN:
                return greenOpinion;
            case CharacterID.RED:
                return redOpinion;
            case CharacterID.YELLOW:
                return yellowOpinion;
            default:
                return "I don't know who you're talking about.";
        }
    }

    public string GetFirstHint()
    {
        if (hasHint)
        {
            Character suspect = DialogueSystem.Instance().characters.IDToCharacter(hintBlames);
            string hint = quotes.startHintYes[Random.Range(0, quotes.startHintYes.Count)];
            //Regex suspectPattern = new Regex("/suspect/i");
            hint = Regex.Replace(hint, "Suspect", suspect.identifier);
            return hint;
        }
        else
        {
            return quotes.startHintNo[Random.Range(0, quotes.startHintNo.Count)];
        }
    }

    public string CheckClue(string clueID)
    {
        ClueEntry response;
        try
        {
            if (!clueResponses.TryGetValue(clueID, out response))
            {
                return quotes.genericDontKnow[Random.Range(0, quotes.genericDontKnow.Count)];
            }

        }
        catch
        {
            return quotes.startHintYes[Random.Range(0, quotes.startHintYes.Count)];
        }
        DialogueSystem.Instance().CreateJournalEntry(response.summary, id, response.nextClueID);
        return response.text;
    }

    public string ReplaceSuspect(string line, CharacterID suspect)
    {
        Character suspectCharacter = DialogueSystem.Instance().characters.IDToCharacter(suspect);
        string hint = line;
        return Regex.Replace(hint, "Suspect", suspectCharacter.identifier);
    }

    IEnumerator ExamineBody()
    {
        DialogueSystem.Instance().CreateJournalEntry("The victim was [Victim].", CharacterID.VICTIM, "motive");
        yield return new WaitForSeconds(0.05f);
        DialogueSystem.Instance().CreateJournalEntry("[Victim] was strangled.", CharacterID.VICTIM, "method");
        yield return new WaitForSeconds(0.05f);
        DialogueSystem.Instance().CreateJournalEntry("[Victim] was killed in the Town Square.", CharacterID.VICTIM, "opportunity");
    }
}
