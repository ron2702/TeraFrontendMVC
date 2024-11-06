namespace TeraFrontendMVC.Models.Region
{
    public class SelectsViewModel
    {
        public List<StateViewModel> States { get; set; }
        public List<MunicipalityViewModel> Municipalities { get; set; }
        public List<ParishViewModel> Parishes { get; set; }
    }
}
