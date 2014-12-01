angular.module('myApp', [])
    .controller('searchCtrl', [
        '$scope', '$http',
        function($scope, $http) {

            $scope.categories = [
                { name: 'Category 1', id: 1 },
                { name: 'Category 2', id: 2 },
                { name: 'Category 3', id: 3 }
            ];

            //Properties
            $scope.searchTerm = $('#searchTerm').val();
            $scope.notContains = '';
            $scope.createdAfter = new Date(2000, 1, 1);
            $scope.createdBefore = new Date();
            $scope.category = { name: 'Select Category', id: -1 };
            $scope.orderby = {
                fieldName: 'RELEVANCE',
                displayName: 'Please Select'
            };

            //UI 
            $scope.showNotContains = false;
            $scope.showCreatedAfter = false;
            $scope.showCreatedBefore = false;
            $scope.showCategory = false;

            $scope.results = [];

            $scope.selectOrderBy = function(fieldName, displayName) {
                $scope.orderby = {
                    fieldName: fieldName,
                    displayName: displayName
                };
            };

            $scope.selectCategory = function(category) {
                $scope.category = category;
            };

            $scope.submit = function() {
                $('#results').hide();
                var queryString = '?term=' + $scope.searchTerm;

                if ($scope.showNotContains) {
                    queryString += '&notContains=' + $scope.notContains;
                }
                if ($scope.showCreatedAfter) {
                    queryString += '&createdAfter=' + $scope.createdAfter.getTime();
                }
                if ($scope.showCreatedBefore) {
                    queryString += '&createdBefore=' + $scope.createdBefore.getTime();
                }
                if ($scope.showCategory && $scope.category.id > -1) {
                    queryString += '&category=' + $scope.category.id;
                }

                queryString += '&sortBy=' + $scope.orderby.fieldName;

                $http.get('/api/advancedSearch' + queryString)
                    .success(function(data) {
                        $scope.results = data;
                        $('#results').fadeIn();
                    });
            };

            $scope.basicSearch = function() {
                $('#results').hide();
                $http.get('/api/advancedSearch?term=' + $scope.searchTerm)
                    .success(function(data) {
                        $scope.results = data;
                        $('#results').fadeIn();
                    });
            };

            $scope.basicSearch();
        }
    ]);