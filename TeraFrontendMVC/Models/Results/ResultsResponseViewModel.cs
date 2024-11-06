namespace TeraFrontendMVC.Models.Results
{
    public class ResultsResponseViewModel
    {
        public int Amount { get; set; }
        public List<ResultsViewModel> Resultados { get; set; }
        public TotalVotesViewModel SumaVotos { get; set; }
        public string PageNumber { get; set; }
    }
}
