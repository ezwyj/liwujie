using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using DogNet.Repositories;

namespace Get.Entity
{
    
    [TableName("liewushuosourcepage")]
    [PrimaryKey("ID", autoIncrement = true)]
    public class SourcePageEntity:Repository<SourcePageEntity>
    {
        public UInt32 ID { get; set; }
        /// <summary>
        /// 送礼目标
        /// </summary>
        public string Target { get; set; }
        /// <summary>
        /// 场合
        /// </summary>
        public string Scene { get; set; }
        /// <summary>
        /// 个性
        /// </summary>
        public string Personality { get; set; }
        /// <summary>
        /// 原始页面
        /// </summary>
        public string Page { get; set; }
        /// <summary>
        /// 封面图片地址
        /// </summary>
        public string cover_image_url { get; set; }

        /// <summary>
        /// 本地存储页
        /// </summary>
        public string LocalPage { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
    }
}
