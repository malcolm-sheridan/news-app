<template>
  <div id="app">    
    <Guardian />
  </div>
</template>

<script>
import Guardian from './components/Guardian.vue'

export default {
  name: 'App',
  components: {    
    Guardian
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;  
  color: #000000;  
}
</style>
