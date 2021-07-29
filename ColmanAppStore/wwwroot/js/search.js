
$(function () {
    $('form').submit(function (e) {
        e.preventDefault();

        var query = $('#query').val();

        $.ajax({
            //method : 'post',
            url: '/Users/SearchUser',
            data: { 'query': query }
        }).done(function (data) {

            $('tbody').html('');

            var template = $('#hidden-template').html();

            $.each(data, function (i, val) {

                var temp = template;
                $.each(val, function (key, value) {
                    if (key.includes("userType")) //updating the usertype by name (instead of number)
                    {
                        if (value == 0)
                            value = "Client";
                        else if (value == 1) {
                            value = "Programmer"
                        }
                        else if (value == 2) {
                            value = "Admin"
                        }
                    }
                    temp = temp.replaceAll('{' + key + '}', value);
                });

                $('tbody').append(temp);
            });
        });
    });
});



