<template>
  <div class="container">
    <div class="row">
      <div class="col">
        <h1>Search Guardian</h1>       
        <input type="text" v-model="searchTerm" v-on:keyup="search" placeholder="Enter Guardian search" />&nbsp;
        <button v-on:click="search" :disabled="this.searchTerm.trim().length === 0" class="btn btn-primary btn-sm">Search</button>&nbsp;
        <button v-on:click="clear" class="btn btn-primary btn-sm">Clear</button>
        <img src="../assets/ajax-loader.gif" class="loader" v-show="isSearching" />
      </div>
      <div class="col">
        <h1>Pinned Results {{ this.pinnedSearchResults.length }}</h1> 
      </div>
    </div>
    
    <br />

    <div class="row">
      <div class="col">
        <div v-show="emptyData">
          No results...
        </div>

        <div v-for="item in guardianGroupedData" :key="item.Section">
          <div class="alert alert-primary" role="alert">
            {{ item.Section }}
          </div>
          
          <div v-for="d in item.Results" :key="d.Results" class="padding">            
            <a :href="d.WebUrl" target="_blank">{{d.WebTitle}}</a> - {{ d.WebPublicationDate | formatDate }} <button v-on:click="pinSearchResult(d)" class="btn btn-primary btn-sm">pin</button><br />
          </div>
        </div>
      </div> 

      <div class="col">        
        <div v-for="item in pinnedSearchResults" :key="item.Section" class="padding">
          <a :href="item.WebUrl" target="_blank">{{item.WebTitle}}</a> - {{ item.WebPublicationDate | formatDate }} - {{ item.SectionName }}
        </div>
      </div>   
    </div>    
  </div>
</template>

<script>

import axios from 'axios'
import _ from 'lodash'
import moment from 'moment'

export default {
  name: 'Guardian',  
  data() {
    return {
      isSearching: false,
      emptyData: false,
      searchTerm: '',
      guardianGroupedData: [],
      pinnedSearchResults: []
    }
  },  
  filters: {
    formatDate: function (value) {
      return moment(value).format('DD/MM/YYYY')
    }
  },
  methods: {
    search: _.debounce(function () {
      this.isSearching = true
      let url = `https://guardianapitest.azurewebsites.net/api/SearchTheGuardian?code=ooLa3/0rFMqDBBfbu/MSOAx6s5ccYPRQ2UoXVqqfBduVnOr8mVKzpg==&searchTerm=${this.searchTerm}`
      this.guardianGroupedData = []

      if(this.searchTerm.trim().length === 0) {
        this.isSearching = false
        return false;
      }
      
      axios.get(url)
        .then(r => {           
            if(r.data.length === 0) {
              this.emptyData = true
            } else {
              this.emptyData = false
            }           

            this.guardianGroupedData = r.data   
            this.isSearching = false
        })
        .catch(e => {
            this.isSearching = false
            alert(e)
        }) }, 
    750),  
    clear: function () {
      this.guardianGroupedData = []
      this.searchTerm = ''
      this.emptyData = false
    },  
    pinSearchResult: function (e) {
      let pinnedItemExists = _.find(this.pinnedSearchResults, function(o) { return o.Id === e.Id; });

      if(! pinnedItemExists) {
        this.pinnedSearchResults.push(e)      
      }      
    }
  }
}
</script>

<style scoped>
.loader {
  padding-left: 10px;
}
.padding {
  padding: 5px 0 5px 0;
}
</style>