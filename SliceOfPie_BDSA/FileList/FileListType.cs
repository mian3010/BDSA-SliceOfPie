﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SliceOfPie_Model {
  public enum FileListType {
    ClientToServer,
    ServerToClient,
    Conflict,
      Push,
      Pull
  };
}
