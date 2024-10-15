using BusinessObject.Entity;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }
        public void Add(Tag tag)
        {
            _tagRepository.Add(tag);
        }

        public List<Tag> GetAllTag()
        {
            return _tagRepository.GetAllTag();
        }

        public Tag? GetTagById(int tagId)
        {
            return _tagRepository.GetTagById(tagId);
        }
    }
}
