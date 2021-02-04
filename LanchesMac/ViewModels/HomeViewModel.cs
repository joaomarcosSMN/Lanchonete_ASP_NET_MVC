using System.Collections.Generic;
using LanchesMac.Models;

namespace LanchesMac.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Lanche> LanchesPreferidos { get; set; }
    }
}