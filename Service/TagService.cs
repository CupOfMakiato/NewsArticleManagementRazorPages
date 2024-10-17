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
        public void AddTag(Tag tag)
        {
            _tagRepository.AddTag(tag);
        }
        public void UpdateTag(Tag tag)
        {
            _tagRepository.UpdateTag(tag);
        }
        public void DeleteTag(Tag tag)
        {
            _tagRepository.DeleteTag(tag);
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
