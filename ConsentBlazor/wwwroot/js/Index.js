function returnArrayAsync()  {
    DotNet.invokeMethodAsync('ConsentBlazor', 'ReturnArrayAsync')
        .then(data => {
            console.log(data);
        });
}