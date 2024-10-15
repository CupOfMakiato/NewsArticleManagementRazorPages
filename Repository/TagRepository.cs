using BusinessObject.Entity;
using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class TagRepository : ITagRepository
    {
        public void Add(Tag tag)
        {
            TagDAO.getInstance().Add(tag);
        }

        public Tag? GetTagById(int tagId)
        {
            return TagDAO.getInstance().GetTagById(tagId);
        }

        public List<Tag> GetAllTag()
        {
            return TagDAO.getInstance().GetAllTag();
        }
    }
}
