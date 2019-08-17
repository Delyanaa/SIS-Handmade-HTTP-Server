# SIS-Handmade-HTTP-Server

A very simple HTTP Server. 

<ol>
  <li>
    <h3><i>Solution Architecture </h3>
      <ul>
        <li>SIS.HTTP</li>
        <li>SIS.WebServer</li>
      </ul>
  </li>


  <li>
    <h3><i>SIS.HTTP Project Architecture </h3>
    <p>
      The HTTP Project holds all of the models (and their interfaces) which are used to implement the HTTP Communication over the TCP Link       between the Client and our Server. Naturally, you can work with plain strings and byte arrays, but it will be much more comfortable if     everything has its own class and its own place in the code.
     </p>
  </li>


  <li>
    <h3><i>SIS.WebServer Project Architecture </h3>
    <p> 
      The WebServer Project holds the main classes that establish the connection over TCP Link. These classes use the ones from the HTTP         Project. The Project exposes several classes, which should be used from the outside, in order to implement an application.
    </p>
  </li>
</ol>
