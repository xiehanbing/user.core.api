using System;
using System.Collections.Generic;
using System.Linq;
using Project.Domain.Events;

namespace Project.Domain.AggregatesModel
{
    public class Project:Entity
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        public string  Company { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string  Introduction { get; set; }
        /// <summary>
        /// 融资阶段
        /// </summary>
        public int  FinStage { get; set; }
        /// <summary>
        /// 融资 百分比
        /// </summary>
        public decimal FinPercentage { get; set; }
        /// <summary>
        /// 融资金额 单位 万
        /// </summary>
        public decimal FinMoney { get; set; }
        /// <summary>
        /// 省份id
        /// </summary>
        public int  ProvinceId { get; set; }
        /// <summary>
        /// 省份名称
        /// </summary>
        public string  ProvinceName { get; set; }
        /// <summary>
        /// 城市id
        /// </summary>
        public int  CityId { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        public string  CityName { get; set; }
        /// <summary>
        /// 收益
        /// </summary>
        public decimal Revenue { get; set; }
        /// <summary>
        /// 估价
        /// </summary>
        public decimal Valution { get; set; }
        /// <summary>
        /// 源
        /// </summary>
        public int  SourceId { get; set; }
        /// <summary>
        /// 参考
        /// </summary>
        public int  ReferenceId { get; set; }
        /// <summary>
        /// 属性
        /// </summary>
        public List<ProjectProperty> Properties { get; set; }
        /// <summary>
        /// 可以范围设置
        /// </summary>
        public ProjectVisibleRule VisibleRule { get; set; }
        /// <summary>
        /// 参与者
        /// </summary>
        public List<ProjectContributor> Contributors { get; set; }
        /// <summary>
        /// 查看者
        /// </summary>
        public List<ProjectViewer> Viewers { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int  UserId { get; set; }
        /// <summary>
        /// 区域id
        /// </summary>
        public int  AreaId { get; set; }
        /// <summary>
        /// 佣金分配方式
        /// </summary>
        public int  BrokerageOptions { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string  Avatar { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegisterTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string  ShowSecurityInfo { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tags { get; set; }
        public string  Income { get; set; }
        /// <summary>
        /// 是否委托给finbook
        /// </summary>
        public string  OnPlatform { get; set; }
        public string  OriginBpFile { get; set; }
        private Project Clone(Project source = null)
        {
            if (source == null)
                source = this;

            var newProject=new Project()
            {
                AreaId = source.AreaId,
                BrokerageOptions = source.BrokerageOptions,
                Avatar = source.Avatar,
                CityId = source.CityId,
                CityName = source.CityName,
                Company = source.Company,
                Contributors = new List<ProjectContributor>(),
                Viewers = new List<ProjectViewer>(),
                CreatedTime = DateTime.Now,
                FinMoney = source.FinMoney,
                FinStage = source.FinStage,
                FinPercentage=source.FinPercentage,
                RegisterTime = source.RegisterTime,
                Revenue = source.Revenue,
                ShowSecurityInfo = source.ShowSecurityInfo,
                ReferenceId = source.ReferenceId,
                SourceId = source.SourceId,
                Tags = source.Tags,
                ProvinceId = source.ProvinceId,
                ProvinceName = source.ProvinceName,
                VisibleRule = source.VisibleRule==null?null:
                    new ProjectVisibleRule()
                    {
                        Visible = source.VisibleRule.Visible,
                        Tags = source.VisibleRule.Tags
                    },
                Valution = source.Valution,
                Income = source.Income,
                OnPlatform = source.OnPlatform,
                OriginBpFile = source.OriginBpFile
            };
            newProject.Properties=new List<ProjectProperty>();
            foreach (var item in source.Properties)
            {
                newProject.Properties.Add(new ProjectProperty(item.Key,item.Text,item.Value));
            }

            return newProject;
        }


        public Project ContributorFork(int contributorId, Project source = null)
        {
            if (source == null)
                source = this;
            var newProject = Clone(source);
            newProject.UserId = contributorId;
            newProject.SourceId = source.SourceId == 0 ? source.Id : source.SourceId;
            newProject.ReferenceId = source.ReferenceId == 0 ? source.Id : source.ReferenceId;
            newProject.UpdateTime=DateTime.Now;

            return newProject;
        }


        public Project()
        {
            this.Viewers=new List<ProjectViewer>();
            this.Contributors=new List<ProjectContributor>();
            this.AddDomainEvent(new ProjecCreatedtEvent(){Project = this});
        }

        public void AddViewer(int userId,string userName,string avatar)
        {
            var viewer=new ProjectViewer()
            {
                UserId = userId,
                UserName = userName,
                Avatar = avatar,
                CreatedTime = DateTime.Now
            };

            if (!Viewers.Any(v => v.UserId == UserId))
            {
                Viewers.Add(viewer);
                AddDomainEvent(new ProjectViewEvent()
                {
                    Company = this.Company,
                    Introduction = this.Introduction,
                    Viewer = viewer
                });
            }
        
        }

        public void AddContributor(ProjectContributor contributor)
        {
            if (!Contributors.Any(v => v.UserId == UserId))
            {
                Contributors.Add(contributor);
                AddDomainEvent(new ProjectJoinedEvent()
                {
                    Company = this.Company,
                    Introduction = this.Introduction,
                    Contributor = contributor
                });
            }
        }
    }
}