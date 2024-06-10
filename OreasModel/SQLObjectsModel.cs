using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace OreasModel
{
    #region Table Value Functions

    [Keyless]
    public class UDTVF_UserAuthorizedOperations
    {
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanViewReport { get; set; }
        public bool CanViewOnlyOwnData { get; set; }

    }

    #endregion
}
