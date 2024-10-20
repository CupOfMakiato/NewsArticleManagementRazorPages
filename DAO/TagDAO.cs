﻿using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public partial class TagDAO
    {
        private static TagDAO instance = null;
        public TagDAO()
        {

        }
        public static TagDAO getInstance()
        {
            if (instance == null)
            {
                instance = new TagDAO();
            }
            return instance;
        }
        public List<Tag> GetAllTag()
        {
            var listTags = new List<Tag>();
            try
            {
                using var context = new FunewsManagementFall2024Context();
                listTags = context.Tags.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return listTags;
        }

        public Tag? GetTagById(int tagId)
        {
            try
            {
                using var context = new FunewsManagementFall2024Context();
                Tag? tag = context.Tags.Find(tagId);
                return tag;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AddTag(Tag tag)
        {
            try
            {
                using var db = new FunewsManagementFall2024Context();
                db.Tags.Add(tag);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void UpdateTag(Tag tag)
        {
            try
            {
                using var db = new FunewsManagementFall2024Context();
                db.Entry<Tag>(tag).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void DeleteTag(Tag tag)
        {
            try
            {
                using var db = new FunewsManagementFall2024Context();
                var p1 = db.Tags.SingleOrDefault(x => x.TagId == tag.TagId);
                db.Tags.Remove(p1);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
