using System;
using System.Collections.Generic;
using Talento.Entities;

namespace Talento.Core
{
    public interface IComment
    {
        int Create(Comment newComment);
        List<Comment> Get(int CandidateId, int PositionId);
        List<Comment> Get(int PositionId);
        List<Comment> GetAll(int PositionId);
    }
}
