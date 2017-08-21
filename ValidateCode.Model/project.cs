namespace ValidateCode.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("project")]
    public partial class project : base_entity
    {

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        [Required]
        [StringLength(200)]
        public string description { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? price { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? price_discount { get; set; }

        public int country_code { get; set; } = 42;

        public int stock_sum { get; set; } = 0;

        public int sort_index { get; set; } = 0;

        public int? payment_type { get; set; }

        public DateTime create_time { get; set; }

        public DateTime? last_modify_time { get; set; }

        [StringLength(50)]
        public string inbox_filter_name { get; set; }

        [StringLength(50)]
        public string send_filter_name { get; set; }

        public int? server_statu { get; set; }


        [StringLength(126)]
        public string url { get; set; }


        public ProjectState project_state { get; set; }

        /// <summary>
        /// 抽成方式  1-按固定金额  2-按比例
        /// </summary>
        public DivideType divide_type { get; set; } = DivideType.money;

        /// <summary>
        /// 固定抽成金额
        /// </summary>
        [Column(TypeName = "numeric")]
        public decimal? divide_fixed_amount { get; set; }

        /// <summary>
        /// 比例值
        /// </summary>
        public int? divide_ratio_value { get; set; }

    }

    public enum DivideType
    {
        money = 1,
        persent = 2,
    }


    public enum ProjectState
    {
        enable=1200,
        disable=1201,
    }


    public enum CountryCode
    {
//        阿富汗=
//奥兰群岛=
//阿尔巴尼亚=
//阿尔及利亚=
//美属萨摩亚 =
//安道尔 =
//安哥拉 =
//安圭拉 =
//南极洲 =
//安提瓜和巴布达 =
//阿根廷 =
//亚美尼亚 =
//阿鲁巴 =
//澳大利亚 =
//奥地利 =
//阿塞拜疆 =
//B
//巴哈马 =
//巴林 =
//孟加拉国 =
//巴巴多斯 =
//白俄罗斯 =
//比利时 =
//伯利兹 =
//贝宁 =
//百慕大 =
//不丹 =
//玻利维亚 =
//波黑 =
//博茨瓦纳 =
//布维岛 =
//巴西 =
//英属印度洋领地 =
//文莱 =
//保加利亚 =
//布基纳法索 =
//布隆迪 =
//C
//柬埔寨 =
//喀麦隆 =
//加拿大 =
//佛得角 =
//开曼群岛 =
//中非 =
//乍得 =
//智利 =
//中国 =
//圣诞岛 =
//科科斯（基林）群岛 =
//哥伦比亚 =
//科摩罗 =
//刚果（布）=
//刚果（金）=
//库克群岛 =
//哥斯达黎加 =
//科特迪瓦 =
//克罗地亚 =
//古巴 =
//塞浦路斯 =
//捷克 =
//D
//丹麦 =
//吉布提 =
//多米尼克 =
//多米尼加 =
//E
//厄瓜多尔 =
//埃及 =
//萨尔瓦多 =
//赤道几内亚 =
//厄立特里亚 =
//爱沙尼亚 =
//埃塞俄比亚 =
//F
//福克兰群岛（马尔维纳斯）=
//法罗群岛 =
//斐济 =
//芬兰 =
//法国 =
//法属圭亚那 =
//法属波利尼西亚 =
//法属南部领地 =
//G
//加蓬 =
//冈比亚 =
//格鲁吉亚 =
//德国 =
//加纳 =
//直布罗陀 =
//希腊 =
//格陵兰 =
//格林纳达 =
//瓜德罗普 =
//关岛 =
//危地马拉 =
//格恩西岛 =
//几内亚 =
//几内亚比绍 =
//圭亚那 =
//H
//海地 =
//赫德岛和麦克唐纳岛 =
//梵蒂冈 =
//洪都拉斯 =
//香港 =
//匈牙利 =
//I 
//冰岛 =
//印度 =
//印度尼西亚 =
//伊朗 =
//伊拉克 =
//爱尔兰 =
//英国属地曼岛 =
//以色列 =
//意大利 =
//J
//牙买加 =
//日本 =
//泽西岛 =
//约旦 =
//K
//哈萨克斯坦 =
//肯尼亚 =
//基里巴斯 =
//朝鲜 =
//韩国 =
//科威特 =
//吉尔吉斯斯坦 =
//L
//老挝 =
//拉脱维亚 =
//黎巴嫩 =
//莱索托 =
//利比里亚 =
//利比亚 =
//列支敦士登 =
//立陶宛 =
//卢森堡 =
//M
//澳门 =
//前南马其顿 =
//马达加斯加 =
//马拉维 =
//马来西亚 =
//马尔代夫 =
//马里 =
//马耳他 =
//马绍尔群岛 =
//马提尼克 =
//毛利塔尼亚 =
//毛里求斯 =
//马约特 =
//墨西哥 =
//密克罗尼西亚联邦 =
//摩尔多瓦 =
//摩纳哥 =
//蒙古 =
//黑山 =
//蒙特塞拉特 =
//摩洛哥 =
//莫桑比克 =
//缅甸 =
//N
//纳米比亚 =
//瑙鲁 =
//尼泊尔 =
//荷兰 =
//荷属安的列斯 =
//新喀里多尼亚 =
//新西兰 =
//尼加拉瓜 =
//尼日尔 =
//尼日利亚 =
//纽埃 =
//诺福克岛 =
//北马里亚纳 =
//挪威 =
//O
//阿曼 =
//P
//巴基斯坦 =
//帕劳 =
//巴勒斯坦 =
//巴拿马 =
//巴布亚新几内亚 =
//巴拉圭 =
//秘鲁 =
//菲律宾 =
//皮特凯恩 =
//波兰 =
//葡萄牙 =
//波多黎各 =
//Q
//卡塔尔 =
//R
//留尼汪 =
//罗马尼亚 =
//俄罗斯联邦 =
//卢旺达 =
//S
//圣赫勒拿 =
//圣基茨和尼维斯 =
//圣卢西亚 =
//圣皮埃尔和密克隆 =
//圣文森特和格林纳丁斯 =
//萨摩亚 =
//圣马力诺 =
//圣多美和普林西比 =
//沙特阿拉伯 =
//塞内加尔 =
//塞尔维亚 =
//塞舌尔 =
//塞拉利昂 =
//新加坡 =
//斯洛伐克
//斯洛文尼亚 =
//所罗门群岛 =
//索马里 =
//南非 =
//南乔治亚岛和南桑德韦奇岛 =
//西班牙 =
//斯里兰卡 =
//苏丹 =
//苏里南 =
//斯瓦尔巴岛和扬马延岛 =
//斯威士兰 =
//瑞典 =
//瑞士 =
//叙利亚 =
//T
//台湾 =
//塔吉克斯坦 =
//坦桑尼亚 =
//泰国 =
//东帝汶 =
//多哥 =
//托克劳 =
//汤加 =
//特立尼达和多巴哥 =
//突尼斯 =
//土耳其 =
//土库曼斯坦 =
//特克斯和凯科斯群岛 =
//图瓦卢 =
//U
//乌干达 =
//乌克兰 =
//阿联酋 =
//英国 =
//美国 =
//美国本土外小岛屿 =
//乌拉圭 =
//乌兹别克斯坦 =
//V
//瓦努阿图 =
//委内瑞拉 =
//越南 =
//英属维尔京群岛 =
//美属维尔京群岛 =
//W
//瓦利斯和富图纳 =
//西撒哈拉 =
//Y
//也门 =
//Z
//赞比亚 =
//津巴布韦 =

    }
}
