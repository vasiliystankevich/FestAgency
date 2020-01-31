const vm = new Vue({
	el: '#app',   
	//Mock data for the value of BTC in USD
	data: 
	{
		apiUrl: "http://localhost:46631/api/1.0/weather/getdata",
		apiRequestBody:
		{
			ZipCode: "",
			MetricSystem: true			
		},		
		results: []	
	},
	methods:
	{
		find: function(event)
		{
			axios.post(this.apiUrl, this.apiRequestBody)
			.then(response => {
					if (response.data.Status.Code==200)
						this.results = response.data.Information;
				})
			.catch(error => console.log(error));			
		}
	}
});
