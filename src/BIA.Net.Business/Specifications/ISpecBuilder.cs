using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIA.Net.Business.Specifications
{
    public interface ISpecBuilder<Entity,CTO>
    {
        Specification<Entity> BuildSpec(CTO advancedFilter);
    }
}
