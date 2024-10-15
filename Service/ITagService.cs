using BusinessObject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ITagService
    {
        void Add(Tag tag);
        List<Tag> GetAllTag();
        Tag? GetTagById(int tagId);
    }
}
