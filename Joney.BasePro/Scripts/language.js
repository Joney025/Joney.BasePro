document.write('<a name="StranLink" id="StranLink">繁體中文</a>');
//模仿语言包式的简繁转换功能插件！
var Default_isFT = 0;		//默认是否繁体，0-简体，1-繁体
var StranIt_Delay = 50; //翻译延时毫秒（设这个的目的是让网页先流畅的显现出来）

var StranLink_Obj=document.getElementById("StranLink");
if (StranLink_Obj)
{
	var JF_cn="ft"+self.location.hostname.toString().replace(/\./g,"");
	var BodyIsFt=getCookie(JF_cn);
	if(BodyIsFt!="1")BodyIsFt=Default_isFT;
	with(StranLink_Obj)
	{
		if(typeof(document.all)!="object") 	//非IE浏览器
		{
			href="javascript:StranBody()";
		}
		else
		{
			href="#";
			onclick= new Function("StranBody();return false");
		}
		
		title=StranText("点击以繁体中文方式浏览",1,1);
		innerHTML=StranText(innerHTML,1,1);
	}
	if(BodyIsFt=="1"){setTimeout("StranBody()",StranIt_Delay);}
}
//－－－－－－－代码开始，以下别改－－－－－－－
//转换文本
function StranText(txt,toFT,chgTxt)
{
	if(txt==""||txt==null)return "";
	toFT=toFT==null?BodyIsFt:toFT;
	if(chgTxt)txt=txt.replace((toFT?"简":"繁"),(toFT?"繁":"简"));
	if(toFT){return Traditionalized(txt);}
	else {return Simplized(txt);}
}
//转换对象，使用递归，逐层剥到文本
function StranBody(fobj)
{
	if(typeof(fobj)=="object"){var obj=fobj.childNodes;}
	else 
	{
		var tmptxt=StranLink_Obj.innerHTML.toString();
		if(tmptxt.indexOf("简")<0)
		{
			BodyIsFt=1;
			StranLink_Obj.innerHTML=StranText(tmptxt,0,1);
			StranLink_Obj.title=StranText(StranLink_Obj.title,0,1);
		}
		else
		{
			BodyIsFt=0;
			StranLink_Obj.innerHTML=StranText(tmptxt,1,1);
			StranLink_Obj.title=StranText(StranLink_Obj.title,1,1);
		}
		setCookie(JF_cn,BodyIsFt,7);
		var obj=document.body.childNodes;
	}	
	for(var i=0;i<obj.length;i++)
	{
		var OO=obj.item(i);
		if("||BR|HR|TEXTAREA|".indexOf("|"+OO.tagName+"|")>0||OO==StranLink_Obj)continue;
		if(OO.title!=""&&OO.title!=null)OO.title=StranText(OO.title);
		if(OO.alt!=""&&OO.alt!=null)OO.alt=StranText(OO.alt);
		if(OO.tagName=="INPUT"&&OO.value!=""&&OO.type!="text"&&OO.type!="hidden")OO.value=StranText(OO.value);
		if(OO.nodeType==3){OO.data=StranText(OO.data);}
		else StranBody(OO);
	}
}
function JTPYStr()
{
	return '锕皑蔼碍爱嗳嫒瑷暧霭谙铵鹌肮袄奥媪骜鳌坝罢钯摆败颁办绊钣帮绑镑谤绷饱宝报鲍鸨龅剥拨钵铂驳饽钹鹁辈贝钡狈备惫呗鹎锛笔毕毙币闭荜哔滗贲铋筚跸纰罴铍边编贬变辩辫苄缏笾标骠飑飙镖镳鳔鳖别瘪濒滨宾摈傧缤槟殡膑镔髌鬓饼禀补钸财参蚕残惭惨灿骖黪苍舱仓沧伧厕侧册测恻层诧锸侪钗虿搀掺蝉馋谗缠铲产阐颤冁谄蒇忏婵骣觇禅场尝长偿肠厂畅伥苌怅阊鲳钞车彻砗尘陈衬谌谶榇碜龀撑称惩诚骋枨柽铖铛蛏痴迟驰耻齿炽饬鸱师狮湿诗时蚀实识驶势适释饰视试谥埘莳弑轼贳铈鲥冲虫宠铳畴踌筹绸俦帱雠橱厨锄雏础储触处刍绌蹰传钏疮闯创怆桩庄装妆壮状锤纯鹑绰辍龊辞词赐鹚聪葱囱从丛苁骢枞凑辏蹿窜撺错锉鹾达哒鞑带贷绐担单郸掸胆惮诞殚赕瘅箪弹摊贪瘫滩坛谭谈叹昙钽锬当挡党荡档谠砀裆捣岛祷导盗焘灯邓镫敌涤递缔籴诋谛觌镝斋债颠点垫电巅钿癫钓鲷调条粜龆鲦谍叠鲽钉顶锭订丢铥东动栋冻岽鸫钭窦犊独读赌镀渎椟牍笃黩锻断缎簖兑队对怼镦吨顿钝炖趸饨夺堕缍铎鹅额讹恶饿谔垩轭锇锷鹗颚鳄儿尔饵贰迩铒鸸鲕发罚阀珐矾钒烦贩饭访纺钫鲂飞诽废费绯镄鲱纷坟奋愤粪偾丰枫锋风疯冯缝讽凤沣肤辐抚辅赋复负讣妇缚凫呒驸绂绋赙麸鲋鳆钆该钙盖赅杆赶秆赣尴擀绀冈刚钢纲岗戆镐睾诰缟锆搁鸽阁铬个纥镉铪给亘赓绠鲠龚宫巩贡钩沟苟构购够诟缑觏蛊顾诂毂钴锢鸪剐挂诖鸹掴关观馆惯贯掼鹳鳏广犷规归龟闺轨诡贵刽匦刿妫桧鲑鳜辊滚衮绲鲧锅国过埚呙帼椁蝈骇韩汉顸颔绗颃号灏颢蚝阂鹤贺诃阖颌横轰鸿红黉讧荭闳鲎壶护沪户浒鹄鹕鹘哗华画划话骅桦铧怀坏欢环还缓换唤痪焕涣奂缳锾鲩黄谎鳇挥辉毁贿秽会烩汇讳诲绘诙荟缋珲晖荤浑诨馄阍获货祸钬镬击机积饥迹讥鸡绩缉极辑级挤几蓟剂济计记际继纪荠叽哜骥玑觊齑矶羁虮跻霁鲚鲫夹荚颊贾钾价驾郏浃铗镓蛱歼监坚笺间艰缄茧检碱硷拣捡简俭减荐槛鉴践贱见键舰剑饯渐溅涧谏缣戋戬睑鹣笕鲣鞯将浆蒋桨奖讲酱绛缰胶浇骄娇搅铰矫侥脚饺缴绞轿较挢峤鹪鲛觉决诀绝谲珏阶节洁结诫届讦诘疖颉鲒紧锦仅谨进晋烬尽劲卺荩馑缙赆觐荆茎鲸惊经颈静镜径痉竞净刭泾迳弪胫纠厩旧阄鸠鹫驹举据锯惧剧讵屦榉飓钜锔窭龃鹃绢锩镌隽钧军骏皲萝罗逻锣箩骡骆络荦猡泺椤脶镙开凯剀垲忾恺铠锴阚龛闶钪铐颗壳课骒缂轲钶锞颏垦恳铿抠库裤喾块侩郐哙狯浍脍宽髋矿旷况诓诳邝圹纩贶亏岿窥馈溃匮蒉愦聩篑阃锟鲲扩阔蜡腊莱来赖崃徕涞濑赉睐铼癞籁蓝栏拦篮阑兰澜谰揽览懒缆烂滥岚榄斓镧褴琅阆捞劳涝唠崂铑铹痨鳓乐约跃粤悦阅哕钺镭垒类泪诔缧篱狸离鲤礼丽厉励砾历沥隶俪郦坜苈莅蓠呖逦骊缡枥栎轹砺锂鹂疠蛎粝跞雳鲡鳢俩联莲连镰怜涟帘敛脸链恋炼练蔹奁潋琏殓裢裣鲢粮凉两辆谅锒靓魉疗辽镣缭钌鹩猎临邻鳞凛赁蔺廪檩辚躏龄铃灵岭领绫棂鲮馏刘浏骝绺镏鹨龙聋咙笼垄拢陇茏珑栊胧砻楼娄搂篓偻蒌喽嵝镂瘘耧蝼髅芦卢颅庐炉掳卤虏鲁赂禄录陆垆撸噜泸渌栌橹轳辂辘氇胪鸬鹭舻鲈驴吕铝侣屡缕虑滤绿闾榈褛峦挛孪滦乱脔娈栾鸾銮锊抡轮伦仑沦纶论囵妈玛码蚂马骂吗唛杩买麦卖迈脉劢瞒馒蛮满谩缦镘颟鳗猫锚铆贸麽镁没谟蓦馍嬷殁镆门闷们扪焖懑钔锰梦眯谜弥觅幂芈谧猕祢绵缅渑腼庙缈灭悯闽闵缗黾鸣铭谬缪谋亩钼呐钠纳讷难挠脑恼闹铙蛲馁内拟腻铌鲵撵辇鲶酿鸟茑袅聂啮镊镍陧蘖嗫颞蹑柠狞宁拧泞咛聍钮纽脓浓农侬哝驽钕疟诺傩欧鸥殴呕沤讴怄瓯盘蹒庞抛疱赔辔喷鹏骗谝骈飘缥频贫嫔颦苹凭评泼颇钋扑铺朴谱镤镨栖脐齐骑岂启气弃讫蕲骐绮桤碛颀蛴鳍牵钎铅迁签谦钱钳潜浅谴堑佥悭骞缱椠钤枪呛墙蔷强抢嫱樯戗炝锖锵镪羟跄锹桥乔侨翘窍诮谯荞缲硗跷窃惬锲箧钦亲寝揿锓轻氢倾顷请庆鲭琼穷茕巯赇鳅趋区躯驱龋诎岖阒觑鸲颧权劝诠绻辁铨却鹊确阕阙悫让饶扰绕荛娆桡热韧认纫饪轫荣绒嵘蝾缛铷软锐闰润洒萨飒鳃赛伞毵丧颡骚扫缫涩啬铯穑杀刹纱铩鲨筛晒酾删闪陕赡缮讪姗骟钐鳝墒伤赏垧泷殇觞烧绍赊摄慑设厍滠绅审婶肾渗诜谂渖糁声绳胜寿兽绶枢输书赎属术树竖数摅纾帅闩双谁税顺说硕烁铄丝饲厮驷缌锶鸶蛳耸怂颂讼诵擞薮馊飕锼苏诉肃谡稣虽随绥岁谇孙损笋荪狲缩琐锁唢睃獭挞闼铊鳎台态骀钛鲐汤烫傥铴镗涛绦讨韬铽腾誊锑题体屉绨缇鹈阗贴铁厅听烃铤铜统恸头秃图钍团抟颓蜕脱鸵驮驼椭箨鼍袜娲腽弯湾顽万纨绾网辋韦违围为潍维苇伟伪纬谓卫诿帏闱沩涠玮韪炜鲔温闻纹稳问阌瓮挝蜗涡窝卧莴龌呜钨乌诬无芜吴坞雾务误邬庑怃妩骛鹉鹜锡牺袭习铣戏细饩阋玺觋虾辖峡侠狭厦吓硖鲜纤贤衔闲显险现献县馅羡宪线苋莶藓岘猃娴鹇痫蚬籼跹厢镶乡详响项芗饷骧缃飨萧嚣销晓啸哓潇骁绡枭箫协挟携胁谐写泻谢亵撷绁缬锌衅镡兴陉荥饧凶汹锈绣馐鸺虚嘘须许叙绪续诩顼轩悬选癣绚谖铉镟学谑泶鳕勋询寻驯训讯逊埙荨浔鲟压鸦鸭哑亚讶垭娅桠氩阉烟盐严岩颜阎艳厌砚彦谚验厣赝俨兖谳恹闫阏酽魇餍鼹鸯杨扬疡阳痒养样炀瑶摇尧遥窑谣药钥轺铫鹞鳐爷页业叶靥谒邺晔烨医铱颐遗仪蚁艺亿忆义诣议谊译异绎诒呓峄饴怿驿缢轶贻钇镒镱瘗舣荫阴银饮隐铟瘾龈樱婴鹰应缨莹萤营荧蝇赢颖茔莺萦蓥撄嘤滢潆璎鹦瘿颍罂哟拥佣痈踊咏镛优忧邮铀犹诱莸铕鱿舆鱼渔娱与屿语狱誉预驭伛俣谀谕蓣嵛饫阈妪纡觎欤畲钰鹆鹬龉鸳渊辕园员圆缘远橼鸢鼋郧匀陨运蕴酝晕韵郓芸恽愠纭韫殒氲杂灾载攒暂赞瓒趱錾赃脏驵凿枣责择则泽赜啧帻箦贼谮赠缯轧铡闸栅诈毡盏斩辗崭栈战绽谵张涨帐账胀赵着诏钊蛰辙锗这谪辄鹧贞针侦诊镇阵帧浈缜桢轸赈祯鸩挣睁狰争症郑证诤峥钲铮筝织职执纸挚掷帜质滞骘栉栀轵轾贽鸷絷踬踯觯钟终种肿众锺诌轴皱昼骤纣绉猪诸诛烛瞩嘱贮铸驻伫苎槠铢专砖转赚啭馔颛锥赘坠缀骓缒谆准浊诼镯兹资渍谘缁辎赀眦锱龇鲻踪综总纵偬邹诹驺鲰诅组镞钻缵躜鳟诶谷布沈采';
}
function FTPYStr()
{
	return '錒皚藹礙愛噯嬡璦曖靄諳銨鵪骯襖奧媼驁鰲壩罷鈀擺敗頒辦絆鈑幫綁鎊謗繃飽寶報鮑鴇齙剝撥缽鉑駁餑鈸鵓輩貝鋇狽備憊唄鵯錛筆畢斃幣閉蓽嗶潷賁鉍篳蹕紕羆鈹邊編貶變辯辮芐緶籩標驃颮飆鏢鑣鰾鱉別癟瀕濱賓擯儐繽檳殯臏鑌髕鬢餅稟補鈽財參蠶殘慚慘燦驂黲蒼艙倉滄傖廁側冊測惻層詫鍤儕釵蠆攙摻蟬饞讒纏鏟產闡顫囅諂蕆懺嬋驏覘禪場嘗長償腸廠暢倀萇悵閶鯧鈔車徹硨塵陳襯諶讖櫬磣齔撐稱懲誠騁棖檉鋮鐺蟶癡遲馳恥齒熾飭鴟師獅濕詩時蝕實識駛勢適釋飾視試謚塒蒔弒軾貰鈰鰣沖蟲寵銃疇躊籌綢儔幬讎櫥廚鋤雛礎儲觸處芻絀躕傳釧瘡闖創愴樁莊裝妝壯狀錘純鶉綽輟齪辭詞賜鶿聰蔥囪從叢蓯驄樅湊輳躥竄攛錯銼鹺達噠韃帶貸紿擔單鄲撣膽憚誕殫賧癉簞彈攤貪癱灘壇譚談嘆曇鉭錟當擋黨蕩檔讜碭襠搗島禱導盜燾燈鄧鐙敵滌遞締糴詆諦覿鏑齋債顛點墊電巔鈿癲釣鯛調條糶齠鰷諜疊鰈釘頂錠訂丟銩東動棟凍崠鶇鈄竇犢獨讀賭鍍瀆櫝牘篤黷鍛斷緞籪兌隊對懟鐓噸頓鈍燉躉飩奪墮綞鐸鵝額訛惡餓諤堊軛鋨鍔鶚顎鱷兒爾餌貳邇鉺鴯鮞發罰閥琺礬釩煩販飯訪紡鈁魴飛誹廢費緋鐨鯡紛墳奮憤糞僨豐楓鋒風瘋馮縫諷鳳灃膚輻撫輔賦復負訃婦縛鳧嘸駙紱紼賻麩鮒鰒釓該鈣蓋賅桿趕稈贛尷搟紺岡剛鋼綱崗戇鎬睪誥縞鋯擱鴿閣鉻個紇鎘鉿給亙賡綆鯁龔宮鞏貢鉤溝茍構購夠詬緱覯蠱顧詁轂鈷錮鴣剮掛詿鴰摑關觀館慣貫摜鸛鰥廣獷規歸龜閨軌詭貴劊匭劌媯檜鮭鱖輥滾袞緄鯀鍋國過堝咼幗槨蟈駭韓漢頇頷絎頏號灝顥蠔閡鶴賀訶闔頜橫轟鴻紅黌訌葒閎鱟壺護滬戶滸鵠鶘鶻嘩華畫劃話驊樺鏵懷壞歡環還緩換喚瘓煥渙奐繯鍰鯇黃謊鰉揮輝毀賄穢會燴匯諱誨繪詼薈繢琿暉葷渾諢餛閽獲貨禍鈥鑊擊機積饑跡譏雞績緝極輯級擠幾薊劑濟計記際繼紀薺嘰嚌驥璣覬齏磯羈蟣躋霽鱭鯽夾莢頰賈鉀價駕郟浹鋏鎵蛺殲監堅箋間艱緘繭檢堿鹼揀撿簡儉減薦檻鑒踐賤見鍵艦劍餞漸濺澗諫縑戔戩瞼鶼筧鰹韉將漿蔣槳獎講醬絳韁膠澆驕嬌攪鉸矯僥腳餃繳絞轎較撟嶠鷦鮫覺決訣絕譎玨階節潔結誡屆訐詰癤頡鮚緊錦僅謹進晉燼盡勁巹藎饉縉贐覲荊莖鯨驚經頸靜鏡徑痙競凈剄涇逕弳脛糾廄舊鬮鳩鷲駒舉據鋸懼劇詎屨櫸颶鉅鋦窶齟鵑絹錈鐫雋鈞軍駿皸蘿羅邏鑼籮騾駱絡犖玀濼欏腡鏍開凱剴塏愾愷鎧鍇闞龕閌鈧銬顆殼課騍緙軻鈳錁頦墾懇鏗摳庫褲嚳塊儈鄶噲獪澮膾寬髖礦曠況誆誑鄺壙纊貺虧巋窺饋潰匱蕢憒聵簣閫錕鯤擴闊蠟臘萊來賴崍徠淶瀨賚睞錸癩籟藍欄攔籃闌蘭瀾讕攬覽懶纜爛濫嵐欖斕鑭襤瑯閬撈勞澇嘮嶗銠鐒癆鰳樂約躍粵悅閱噦鉞鐳壘類淚誄縲籬貍離鯉禮麗厲勵礫歷瀝隸儷酈壢藶蒞蘺嚦邐驪縭櫪櫟轢礪鋰鸝癘蠣糲躒靂鱺鱧倆聯蓮連鐮憐漣簾斂臉鏈戀煉練蘞奩瀲璉殮褳襝鰱糧涼兩輛諒鋃靚魎療遼鐐繚釕鷯獵臨鄰鱗凜賃藺廩檁轔躪齡鈴靈嶺領綾欞鯪餾劉瀏騮綹鎦鷚龍聾嚨籠壟攏隴蘢瓏櫳朧礱樓婁摟簍僂蔞嘍嶁鏤瘺耬螻髏蘆盧顱廬爐擄鹵虜魯賂祿錄陸壚擼嚕瀘淥櫨櫓轤輅轆氌臚鸕鷺艫鱸驢呂鋁侶屢縷慮濾綠閭櫚褸巒攣孿灤亂臠孌欒鸞鑾鋝掄輪倫侖淪綸論圇媽瑪碼螞馬罵嗎嘜榪買麥賣邁脈勱瞞饅蠻滿謾縵鏝顢鰻貓錨鉚貿麼鎂沒謨驀饃嬤歿鏌門悶們捫燜懣鍆錳夢瞇謎彌覓冪羋謐獼禰綿緬澠靦廟緲滅憫閩閔緡黽鳴銘謬繆謀畝鉬吶鈉納訥難撓腦惱鬧鐃蟯餒內擬膩鈮鯢攆輦鯰釀鳥蔦裊聶嚙鑷鎳隉蘗囁顳躡檸獰寧擰濘嚀聹鈕紐膿濃農儂噥駑釹瘧諾儺歐鷗毆嘔漚謳慪甌盤蹣龐拋皰賠轡噴鵬騙諞駢飄縹頻貧嬪顰蘋憑評潑頗釙撲鋪樸譜鏷鐠棲臍齊騎豈啟氣棄訖蘄騏綺榿磧頎蠐鰭牽釬鉛遷簽謙錢鉗潛淺譴塹僉慳騫繾槧鈐槍嗆墻薔強搶嬙檣戧熗錆鏘鏹羥蹌鍬橋喬僑翹竅誚譙蕎繰磽蹺竊愜鍥篋欽親寢撳鋟輕氫傾頃請慶鯖瓊窮煢巰賕鰍趨區軀驅齲詘嶇闃覷鴝顴權勸詮綣輇銓卻鵲確闋闕愨讓饒擾繞蕘嬈橈熱韌認紉飪軔榮絨嶸蠑縟銣軟銳閏潤灑薩颯鰓賽傘毿喪顙騷掃繅澀嗇銫穡殺剎紗鎩鯊篩曬釃刪閃陜贍繕訕姍騸釤鱔墑傷賞坰瀧殤觴燒紹賒攝懾設厙灄紳審嬸腎滲詵諗瀋糝聲繩勝壽獸綬樞輸書贖屬術樹豎數攄紓帥閂雙誰稅順說碩爍鑠絲飼廝駟緦鍶鷥螄聳慫頌訟誦擻藪餿颼鎪蘇訴肅謖穌雖隨綏歲誶孫損筍蓀猻縮瑣鎖嗩脧獺撻闥鉈鰨臺態駘鈦鮐湯燙儻鐋鏜濤絳討韜鋱騰謄銻題體屜綈緹鵜闐貼鐵廳聽烴鋌銅統慟頭禿圖釷團摶頹蛻脫鴕馱駝橢籜鼉襪媧膃彎灣頑萬紈綰網輞韋違圍為濰維葦偉偽緯謂衛諉幃闈溈潿瑋韙煒鮪溫聞紋穩問閿甕撾蝸渦窩臥萵齷嗚鎢烏誣無蕪吳塢霧務誤鄔廡憮嫵騖鵡鶩錫犧襲習銑戲細餼鬩璽覡蝦轄峽俠狹廈嚇硤鮮纖賢銜閑顯險現獻縣餡羨憲線莧薟蘚峴獫嫻鷴癇蜆秈躚廂鑲鄉詳響項薌餉驤緗饗蕭囂銷曉嘯嘵瀟驍綃梟簫協挾攜脅諧寫瀉謝褻擷紲纈鋅釁鐔興陘滎餳兇洶銹繡饈鵂虛噓須許敘緒續詡頊軒懸選癬絢諼鉉鏇學謔澩鱈勛詢尋馴訓訊遜塤蕁潯鱘壓鴉鴨啞亞訝埡婭椏氬閹煙鹽嚴巖顏閻艷厭硯彥諺驗厴贗儼兗讞懨閆閼釅魘饜鼴鴦楊揚瘍陽癢養樣煬瑤搖堯遙窯謠藥鑰軺銚鷂鰩爺頁業葉靨謁鄴曄燁醫銥頤遺儀蟻藝億憶義詣議誼譯異繹詒囈嶧飴懌驛縊軼貽釔鎰鐿瘞艤蔭陰銀飲隱銦癮齦櫻嬰鷹應纓瑩螢營熒蠅贏穎塋鶯縈鎣攖嚶瀅瀠瓔鸚癭潁罌喲擁傭癰踴詠鏞優憂郵鈾猶誘蕕銪魷輿魚漁娛與嶼語獄譽預馭傴俁諛諭蕷崳飫閾嫗紆覦歟畬鈺鵒鷸齬鴛淵轅園員圓緣遠櫞鳶黿鄖勻隕運蘊醞暈韻鄆蕓惲慍紜韞殞氳雜災載攢暫贊瓚趲鏨贓臟駔鑿棗責擇則澤賾嘖幘簀賊譖贈繒軋鍘閘柵詐氈盞斬輾嶄棧戰綻譫張漲帳賬脹趙著詔釗蟄轍鍺這謫輒鷓貞針偵診鎮陣幀湞縝楨軫賑禎鴆掙睜猙爭癥鄭證諍崢鉦錚箏織職執紙摯擲幟質滯騭櫛梔軹輊贄鷙縶躓躑觶鐘終種腫眾鍾謅軸皺晝驟紂縐豬諸誅燭矚囑貯鑄駐佇苧櫧銖專磚轉賺囀饌顓錐贅墜綴騅縋諄準濁諑鐲茲資漬諮緇輜貲眥錙齜鯔蹤綜總縱傯鄒諏騶鯫詛組鏃鉆纘躦鱒誒穀佈瀋採';
}
function Traditionalized(cc){

	var str='',ss=JTPYStr(),tt=FTPYStr();
	for(var i=0;i<cc.length;i++)
	{
		if(cc.charCodeAt(i)>10000&&ss.indexOf(cc.charAt(i))!=-1)str+=tt.charAt(ss.indexOf(cc.charAt(i)));
  		else str+=cc.charAt(i);
	}
	return str;
}
function Simplized(cc){
	var str='',ss=JTPYStr(),tt=FTPYStr();
	for(var i=0;i<cc.length;i++)
	{
		if(cc.charCodeAt(i)>10000&&tt.indexOf(cc.charAt(i))!=-1)str+=ss.charAt(tt.indexOf(cc.charAt(i)));
  		else str+=cc.charAt(i);
	}
	return str;
}

function setCookie(name, value)		//cookies设置
{
	var argv = setCookie.arguments;
	var argc = setCookie.arguments.length;
	var expires = (argc > 2) ? argv[2] : null;
	if(expires!=null)
	{
		var LargeExpDate = new Date ();
		LargeExpDate.setTime(LargeExpDate.getTime() + (expires*1000*3600*24));
	}
	document.cookie = name + "=" + escape (value) + "; path=/;" +((expires == null) ? "" : (" expires=" +LargeExpDate.toGMTString()));
}

function getCookie(Name)			//cookies读取
{
	var search = Name + "=";
	if(document.cookie.length > 0) 
	{
		offset = document.cookie.indexOf(search);
		if(offset != -1) 
		{
			offset += search.length
			end = document.cookie.indexOf(";", offset);
			if(end == -1) end = document.cookie.length;
			return unescape(document.cookie.substring(offset, end));
		 }
	else return ""
	  }
}